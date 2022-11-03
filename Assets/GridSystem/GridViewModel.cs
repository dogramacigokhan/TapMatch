using System.Collections.Generic;

namespace TapMatch.GridSystem
{
    public class GridViewModel
    {
        public readonly GridItemModel[,] GridItemModels;

        private readonly GridItemModelGenerator gridItemModelGenerator;

        public GridViewModel(
            int rowCount,
            int colCount,
            IReadOnlyList<GridItemSetting> gridItemSettings)
        {
            this.gridItemModelGenerator = new GridItemModelGenerator();

            this.GridItemModels = new GridItemModel[rowCount, colCount];
            this.gridItemModelGenerator.GenerateModels(this.GridItemModels, gridItemSettings);
        }
    }
}