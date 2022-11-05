using System;
using System.Collections.Generic;
using DG.Tweening;
using TapMatch.GridSystem.Interactions;
using UnityEngine;
using UnityEngine.Pool;

namespace TapMatch.GridSystem
{
    public class GridView : MonoBehaviour, IInteractionProvider
    {
        [SerializeField] private GridItemView gridItemViewPrefab;
        private GridViewModel viewModel;
        private GridItemView[,] gridItemViews;

        private IObjectPool<GridItemView> itemViewPool;
        private int rowCount;
        private int colCount;

        public event Action<(int row, int column)> GridItemSelected;

        public void Init(GridViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.rowCount = this.viewModel.GridItemModels.GetLength(0);
            this.colCount = this.viewModel.GridItemModels.GetLength(1);

            this.viewModel.AddedGridItems += this.OnAddedGridItems;
            this.viewModel.DestroyedGridItems += this.OnDestroyedGridItems;
            this.viewModel.ShiftedGridItems += this.OnShiftedGridItems;
        }

        private void Start()
        {
            this.gridItemViews = new GridItemView[this.rowCount, this.colCount];
            this.itemViewPool = new ObjectPool<GridItemView>(
                this.CreateGridItemView,
                actionOnGet: view => view.gameObject.SetActive(true),
                actionOnRelease: view => view.gameObject.SetActive(false),
                defaultCapacity: this.rowCount * this.colCount);

            for (var row = 0; row < this.rowCount; row++)
            {
                for (var column = 0; column < this.colCount; column++)
                {
                    var itemView = this.itemViewPool.Get();
                    this.InitGridItemView(itemView, row, column, initialCreation: true);
                }
            }
        }

        private void InitGridItemView(GridItemView view, int row, int column, bool initialCreation)
        {
            var viewTransform = view.transform;
            viewTransform.position = new Vector3(
                x: column - ((float)(this.colCount - 1) / 2),
                y: ((float)(this.rowCount - 1) / 2) - row,
                z: 0);

            var itemModel = this.viewModel.GridItemModels[row, column];
            view.Init(itemModel, row, column);
            view.Clicked += this.OnClickedToItemView;
            this.gridItemViews[row, column] = view;

            if (!initialCreation)
            {
                var initialScale = viewTransform.localScale;
                viewTransform.localScale = Vector3.zero;

                viewTransform
                    .DOScale(initialScale, 0.2f)
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
                var itemView = this.itemViewPool.Get();
                this.InitGridItemView(itemView, indices[i].Row, indices[i].Column, initialCreation: false);
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
                itemView.AnimateOut(onCompleteAction: () => this.itemViewPool.Release(itemView));

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

        private GridItemView CreateGridItemView() => Instantiate(
            this.gridItemViewPrefab,
            Vector3.zero,
            Quaternion.identity,
            this.transform);
    }
}
