using System;
using System.Collections.Generic;
using System.Linq;
using TapMatch.GridSystem.Interactions;
using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridViewModel : IDisposable
    {
        public delegate void AddedOrDestroyedGridItemsEventHandler(GridIndex[] indices);
        public delegate void ShiftedGridItemsEventHandler(Dictionary<GridIndex, int> indicesToShift);

        public readonly GridItemModel[,] GridItemModels;
        public event AddedOrDestroyedGridItemsEventHandler AddedGridItems;
        public event AddedOrDestroyedGridItemsEventHandler DestroyedGridItems;
        public event ShiftedGridItemsEventHandler ShiftedGridItems;

        private readonly GridModel model;
        private readonly GridItemModelGenerator gridItemModelGenerator;
        private readonly IGridMatchFinder gridMatchFinder;
        private readonly IReadOnlyList<IInteractionProvider> interactionProviders;

        private bool areInteractionsEnabled = true;

        public GridViewModel(
            GridModel model,
            IEnumerable<IInteractionProvider> interactionProviders,
            IGridMatchFinder gridMatchFinder)
        {
            this.model = model;
            this.gridItemModelGenerator = new GridItemModelGenerator();
            this.gridMatchFinder = gridMatchFinder;
            this.interactionProviders = interactionProviders.ToList();

            this.GridItemModels = new GridItemModel[this.model.RowCount, this.model.ColCount];
            this.gridItemModelGenerator.GenerateModels(this.GridItemModels, model.GridItemSettings, this.model.ColorCount);

            foreach (var interactionProvider in this.interactionProviders)
            {
                interactionProvider.GridItemSelected += this.OnGridItemSelected;
            }
        }

        public void Dispose()
        {
            foreach (var interactionProvider in this.interactionProviders)
            {
                interactionProvider.GridItemSelected -= this.OnGridItemSelected;
            }
        }

        private void OnGridItemSelected((int selectedRow, int selectedColumn) args)
        {
            if (!this.areInteractionsEnabled)
            {
                Debug.LogWarning("Interactions are suppressed.");
                return;
            }

            var indicesToDestroy = this.gridMatchFinder.FindMatches(
                this.GridItemModels,
                args.selectedRow,
                args.selectedColumn);

            foreach (var index in indicesToDestroy)
            {
                this.GridItemModels[index.Row, index.Column] = null;
            }

            this.DestroyedGridItems?.Invoke(indicesToDestroy);
            this.LocateAndShiftGridItems(indicesToDestroy);
            this.CalculateAndAddGridItems(indicesToDestroy);
        }

        private void LocateAndShiftGridItems(GridIndex[] indicesToDestroy)
        {
            var indicesToShift = new Dictionary<GridIndex, int>();
            foreach (var index in indicesToDestroy)
            {
                for (var i = index.Row - 1; i >= 0; i--)
                {
                    var indexToCheck = new GridIndex(i, index.Column);
                    if (indicesToDestroy.Contains(indexToCheck))
                    {
                        continue;
                    }

                    var newShiftAmount = indicesToShift.TryGetValue(indexToCheck, out var shiftAmount)
                        ? shiftAmount + 1
                        : 1;

                    indicesToShift[indexToCheck] = newShiftAmount;
                }
            }

            foreach (var (index, shiftAmount) in indicesToShift)
            {
                // Shift the item models
                this.GridItemModels[index.Row + shiftAmount, index.Column] = this.GridItemModels[index.Row, index.Column];
            }

            this.ShiftedGridItems?.Invoke(indicesToShift);
        }

        private void CalculateAndAddGridItems(GridIndex[] indicesToDestroy)
        {
            var indicesToAdd = indicesToDestroy
                .GroupBy(index => index.Column)
                .SelectMany(group => Enumerable
                    .Range(0, group.Count())
                    .Select(rowIndex => new GridIndex(rowIndex, column: group.Key)))
                .ToArray();

            foreach (var index in indicesToAdd)
            {
                this.GridItemModels[index.Row, index.Column] =
                    this.gridItemModelGenerator.GenerateModel(this.model.GridItemSettings, this.model.ColorCount);
            }

            this.AddedGridItems?.Invoke(indicesToAdd);
        }

        public void SuppressInteractions(bool shouldSuppress)
        {
            this.areInteractionsEnabled = !shouldSuppress;
        }
    }
}