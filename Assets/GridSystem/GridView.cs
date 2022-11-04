using System.Collections.Generic;
using DG.Tweening;
using TapMatch.GridSystem.Interactions;
using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridView : MonoBehaviour, IInteractionProvider
    {
        [SerializeField] private GridItemView gridItemViewPrefab;
        private GridViewModel viewModel;
        private GridItemView[,] gridItemViews;

        private int rowCount;
        private int colCount;

        public event GridItemSelectedEventHandler GridItemSelected;

        public void Init(GridViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.viewModel.AddedGridItems += this.OnAddedGridItems;
            this.viewModel.DestroyedGridItems += this.OnDestroyedGridItems;
            this.viewModel.ShiftedGridItems += this.OnShiftedGridItems;
        }

        private void Start()
        {
            this.rowCount = this.viewModel.GridItemModels.GetLength(0);
            this.colCount = this.viewModel.GridItemModels.GetLength(1);
            this.gridItemViews = new GridItemView[this.rowCount, this.colCount];

            for (var row = 0; row < this.rowCount; row++)
            {
                for (var column = 0; column < this.colCount; column++)
                {
                    this.CreateGridItemView(row, column, animate: false);
                }
            }
        }

        private void CreateGridItemView(int row, int column, bool animate)
        {
            var gridItemView = Instantiate(
                this.gridItemViewPrefab,
                new Vector3(column - ((float) (this.colCount - 1) / 2), ((float) (this.rowCount - 1) / 2) - row, 0),
                Quaternion.identity,
                this.transform);

            var itemModel = this.viewModel.GridItemModels[row, column];
            gridItemView.Init(itemModel, row, column);
            gridItemView.Clicked += this.OnClickedToItemView;
            this.gridItemViews[row, column] = gridItemView;

            if (animate)
            {
                var viewTransform = gridItemView.transform;
                viewTransform.localScale = Vector3.zero;
                viewTransform.DOScale(Vector3.one, 0.2f).SetDelay(0.2f);
            }
        }

        private void OnClickedToItemView(int row, int column)
        {
            this.viewModel.SuppressInteractions(shouldSuppress: true);
            this.GridItemSelected?.Invoke(row, column);
        }

        private void OnAddedGridItems((int row, int column)[] indices)
        {
            for (var i = 0; i < indices.Length; i++)
            {
                this.CreateGridItemView(indices[i].row, indices[i].column, animate: true);
            }

            this.viewModel.SuppressInteractions(shouldSuppress: false);
        }

        private void OnDestroyedGridItems((int row, int column)[] indices)
        {
            for (var i = 0; i < indices.Length; i++)
            {
                var row = indices[i].row;
                var column = indices[i].column;

                var itemView = this.gridItemViews[row, column];
                itemView.Clicked -= this.OnClickedToItemView;
                itemView.DestroyView();
                this.gridItemViews[row, column] = null;
            }
        }

        private void OnShiftedGridItems(Dictionary<(int row, int col), int> indicesToShift)
        {
            foreach (var indexToShift in indicesToShift)
            {
                var index = indexToShift.Key;
                var shiftAmount = indexToShift.Value;

                var viewToShift = this.gridItemViews[index.row, index.col];
                viewToShift.transform
                    .DOLocalMoveY(-shiftAmount, 0.2f)
                    .SetRelative()
                    .SetDelay(0.2f)
                    .OnComplete(() =>
                    {
                        this.gridItemViews[index.row + shiftAmount, index.col] = viewToShift;
                        viewToShift.ShiftRow(shiftAmount);
                    });
            }
        }
    }
}
