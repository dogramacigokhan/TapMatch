using System.Collections.Generic;
using System.Linq;
using TapMatch.GridSystem.Interactions;
using UnityEngine;

namespace TapMatch.GridSystem
{

    public class GridViewModel
    {
        public delegate void AddOrDestroyGridItemsEventHandler((int row, int column)[] indices);
        public delegate void ShiftGridItemsEventHandler(Dictionary<(int row, int col), int> indicesToShift);

        public readonly GridItemModel[,] GridItemModels;
        public event AddOrDestroyGridItemsEventHandler AddGridItems;
        public event AddOrDestroyGridItemsEventHandler DestroyGridItems;
        public event ShiftGridItemsEventHandler ShiftGridItems;

        private readonly GridItemModelGenerator gridItemModelGenerator;
        private readonly IReadOnlyList<GridItemSetting> gridItemSettings;

        private bool areInteractionsEnabled;

        public GridViewModel(
            int rowCount,
            int colCount,
            IReadOnlyList<GridItemSetting> gridItemSettings,
            IEnumerable<IInteractionProvider> interactionProviders)
        {
            this.gridItemSettings = gridItemSettings;
            this.gridItemModelGenerator = new GridItemModelGenerator();

            this.GridItemModels = new GridItemModel[rowCount, colCount];
            this.gridItemModelGenerator.GenerateModels(this.GridItemModels, gridItemSettings);

            foreach (var interactionProvider in interactionProviders)
            {
                interactionProvider.GridItemSelected += this.OnGridItemSelected;
            }
        }

        private void OnGridItemSelected(int row, int column)
        {
            if (!this.areInteractionsEnabled)
            {
                Debug.LogWarning("Interactions are suppressed.");
            }

            var indicesToDestroy = new (int row, int column)[]
            {
                (row - 2, column - 1),
                (row - 2, column),
                (row - 2, column + 1),
                (row - 1, column - 1),
                (row - 1, column + 1),
                (row, column - 1),
                (row, column),
                (row, column + 1),
            };

            this.DestroyGridItems?.Invoke(indicesToDestroy);

            var indicesToShift = new Dictionary<(int row, int col), int>();
            var elementCountToAddByColumn = indicesToDestroy
                .GroupBy(pair => pair.column)
                .ToDictionary(group => group.Key, group => group.Count());

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

            foreach (var indexToShift in indicesToShift)
            {
                var index = indexToShift.Key;
                var shiftAmount = indexToShift.Value;

                // Shift the item models
                this.GridItemModels[index.row + shiftAmount, index.col] = this.GridItemModels[index.row, index.col];
            }

            this.ShiftGridItems?.Invoke(indicesToShift);

            var indicesToAdd = elementCountToAddByColumn
                .SelectMany(pair => Enumerable
                    .Range(0, pair.Value)
                    .Select(row => (row, column: pair.Key)))
                .ToArray();

            foreach (var tuple in indicesToAdd)
            {
                this.GridItemModels[tuple.row, tuple.column] = this.gridItemModelGenerator.GenerateModel(this.gridItemSettings);
            }

            this.AddGridItems?.Invoke(indicesToAdd);
        }

        public void SuppressInteractions(bool shouldSuppress)
        {
            this.areInteractionsEnabled = shouldSuppress;
        }
    }
}