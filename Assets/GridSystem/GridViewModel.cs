using System.Collections.Generic;
using TapMatch.GridSystem.Interactions;

namespace TapMatch.GridSystem
{
    public delegate void DestroyGridItemsEventHandler(int[] rows, int[] columns);

    public class GridViewModel
    {
        public readonly GridItemModel[,] GridItemModels;
        public event DestroyGridItemsEventHandler DestroyGridItems;

        private readonly GridItemModelGenerator gridItemModelGenerator;

        public GridViewModel(
            int rowCount,
            int colCount,
            IReadOnlyList<GridItemSetting> gridItemSettings,
            IEnumerable<IInteractionProvider> interactionProviders)
        {
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
            this.GridItemModels[row, column] = null;
        }
    }
}