using System.Collections.Generic;
using System.Linq;
using TapMatch.GridSystem.Interactions;
using UnityEngine;

namespace TapMatch.GridSystem
{

    public class GridViewModel
    {
        public delegate void AddedOrDestroyedGridItemsEventHandler((int row, int column)[] indices);
        public delegate void ShiftedGridItemsEventHandler(Dictionary<(int row, int col), int> indicesToShift);

        public readonly GridItemModel[,] GridItemModels;
        public event AddedOrDestroyedGridItemsEventHandler AddedGridItems;
        public event AddedOrDestroyedGridItemsEventHandler DestroyedGridItems;
        public event ShiftedGridItemsEventHandler ShiftedGridItems;

        private readonly GridItemModelGenerator gridItemModelGenerator;
        private readonly IReadOnlyList<GridItemSetting> gridItemSettings;
        private readonly IGridMatchFinder gridMatchFinder;

        private bool areInteractionsEnabled;

        public GridViewModel(
            int rowCount,
            int colCount,
            IReadOnlyList<GridItemSetting> gridItemSettings,
            IEnumerable<IInteractionProvider> interactionProviders,
            IGridMatchFinder gridMatchFinder)
        {
            this.gridItemSettings = gridItemSettings;
            this.gridItemModelGenerator = new GridItemModelGenerator();
            this.gridMatchFinder = gridMatchFinder;

            this.GridItemModels = new GridItemModel[rowCount, colCount];
            this.gridItemModelGenerator.GenerateModels(this.GridItemModels, gridItemSettings);

            foreach (var interactionProvider in interactionProviders)
            {
                interactionProvider.GridItemSelected += this.OnGridItemSelected;
            }
        }

        private void OnGridItemSelected(int selectedRow, int selectedColumn)
        {
            if (!this.areInteractionsEnabled)
            {
                Debug.LogWarning("Interactions are suppressed.");
            }

            var indicesToDestroy = this.gridMatchFinder.FindMatches(
                this.GridItemModels,
                selectedRow,
                selectedColumn);

            foreach (var (row, column) in indicesToDestroy)
            {
                this.GridItemModels[row, column] = null;
            }

            this.DestroyedGridItems?.Invoke(indicesToDestroy);
            this.LocateAndShiftGridItems(indicesToDestroy);
            this.CalculateAndAddGridItems(indicesToDestroy);
        }

        private void LocateAndShiftGridItems((int row, int column)[] indicesToDestroy)
        {
            var indicesToShift = new Dictionary<(int row, int col), int>();
            foreach (var tuple in indicesToDestroy)
            {
                for (var i = tuple.row - 1; i >= 0; i--)
                {
                    var indexToCheck = (i, tuple.column);
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
                this.GridItemModels[index.row + shiftAmount, index.col] = this.GridItemModels[index.row, index.col];
            }

            this.ShiftedGridItems?.Invoke(indicesToShift);
        }

        private void CalculateAndAddGridItems((int row, int column)[] indicesToDestroy)
        {
            var indicesToAdd = indicesToDestroy
                .GroupBy(pair => pair.column)
                .SelectMany(group => Enumerable
                    .Range(0, group.Count())
                    .Select(rowIndex => (rowIndex, column: group.Key)))
                .ToArray();

            foreach (var tuple in indicesToAdd)
            {
                this.GridItemModels[tuple.rowIndex, tuple.column] =
                    this.gridItemModelGenerator.GenerateModel(this.gridItemSettings);
            }

            this.AddedGridItems?.Invoke(indicesToAdd);
        }

        public void SuppressInteractions(bool shouldSuppress)
        {
            this.areInteractionsEnabled = shouldSuppress;
        }
    }
}