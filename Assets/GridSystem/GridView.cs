using System.Collections;
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

            this.viewModel.AddGridItems += this.OnAddGridItems;
            this.viewModel.DestroyGridItems += this.OnDestroyGridItems;
            this.viewModel.ShiftGridItems += this.OnShiftGridItems;
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
            this.GridItemSelected?.Invoke(row, column);
        }

        private void OnAddGridItems(int[] rows, int[] columns)
        {
            for (var i = 0; i < rows.Length; i++)
            {
                this.CreateGridItemView(rows[i], columns[i], animate: true);
            }
        }

        private void OnDestroyGridItems(int[] rows, int[] columns)
        {
            for (var i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                var column = columns[i];

                var itemView = this.gridItemViews[row, column];
                itemView.Clicked -= this.OnClickedToItemView;
                itemView.DestroyView();
                this.gridItemViews[row, column] = null;
            }
        }

        private void OnShiftGridItems(int startRow, int startColumn, int amount)
        {
            for (var i = startRow; i >= 0; i--)
            {
                var viewToShift = this.gridItemViews[i, startColumn];
                this.gridItemViews[i + amount, startColumn] = viewToShift;
                viewToShift.ShiftRow(amount);

                viewToShift.transform
                    .DOLocalMoveY(-amount, 0.2f)
                    .SetRelative()
                    .SetDelay(0.2f);
            }

            for (var i = 0; i < amount; i++)
            {
                this.gridItemViews[i, startColumn] = null;
            }
        }
    }
}
