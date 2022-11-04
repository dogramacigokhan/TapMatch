using System;
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

        public event Action<(int row, int column)> GridItemSelected;

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
                    this.CreateGridItemView(row, column, initialCreation: true);
                }
            }
        }

        private void CreateGridItemView(int row, int column, bool initialCreation)
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

            if (!initialCreation)
            {
                var viewTransform = gridItemView.transform;
                viewTransform.localScale = Vector3.zero;

                viewTransform
                    .DOScale(Vector3.one, 0.2f)
                    .SetDelay(0.4f)
                    .OnComplete(() => this.viewModel.SuppressInteractions(shouldSuppress: false));
            }
        }

        private void OnClickedToItemView(int row, int column)
        {
            this.GridItemSelected?.Invoke((row, column));
        }

        private void OnAddedGridItems(GridIndex[] indices)
        {
            for (var i = 0; i < indices.Length; i++)
            {
                this.CreateGridItemView(indices[i].Row, indices[i].Column, initialCreation: false);
            }
        }

        private void OnDestroyedGridItems(GridIndex[] indices)
        {
            for (var i = 0; i < indices.Length; i++)
            {
                var row = indices[i].Row;
                var column = indices[i].Column;

                var itemView = this.gridItemViews[row, column];
                itemView.Clicked -= this.OnClickedToItemView;
                itemView.DestroyView();
                this.gridItemViews[row, column] = null;
            }

            // Suppress interactions until the new items are added.
            this.viewModel.SuppressInteractions(shouldSuppress: true);
        }

        private void OnShiftedGridItems(Dictionary<GridIndex, int> indicesToShift)
        {
            foreach (var indexToShift in indicesToShift)
            {
                var index = indexToShift.Key;
                var shiftAmount = indexToShift.Value;

                var viewToShift = this.gridItemViews[index.Row, index.Column];
                viewToShift.transform
                    .DOLocalMoveY(-shiftAmount, 0.2f)
                    .SetRelative()
                    .SetDelay(0.2f)
                    .OnComplete(() =>
                    {
                        this.gridItemViews[index.Row + shiftAmount, index.Column] = viewToShift;
                        viewToShift.ShiftRow(shiftAmount);
                    });
            }
        }
    }
}
