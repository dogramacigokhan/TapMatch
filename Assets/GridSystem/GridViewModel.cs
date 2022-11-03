using System.Collections.Generic;
using TapMatch.GridSystem.Interactions;

namespace TapMatch.GridSystem
{

    public class GridViewModel
    {
        public delegate void AddOrDestroyGridItemsEventHandler(int[] rows, int[] columns);
        public delegate void ShiftGridItemsEventHandler(int startRow, int startColumn, int amount);

        public readonly GridItemModel[,] GridItemModels;
        public event AddOrDestroyGridItemsEventHandler AddGridItems;
        public event AddOrDestroyGridItemsEventHandler DestroyGridItems;
        public event ShiftGridItemsEventHandler ShiftGridItems;

        private readonly GridItemModelGenerator gridItemModelGenerator;
        private readonly IReadOnlyList<GridItemSetting> gridItemSettings;

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
            this.DestroyGridItems?.Invoke(new[] {row}, new[] {column});

            for (var i = row - 1; i >= 0; i--)
            {
                this.GridItemModels[i + 1, column] = this.GridItemModels[i, column];
            }

            this.ShiftGridItems?.Invoke(row - 1, column, amount: 1);

            this.GridItemModels[0, column] = this.gridItemModelGenerator.GenerateModel(this.gridItemSettings);
            this.AddGridItems?.Invoke(new[] {0}, new[] {column});
        }
    }
}