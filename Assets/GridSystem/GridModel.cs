using System.Collections.Generic;

namespace TapMatch.GridSystem
{
    public class GridModel
    {
        public readonly int RowCount;
        public readonly int ColCount;
        public readonly int ColorCount;
        public readonly IReadOnlyList<IGridItemSetting> GridItemSettings;

        public GridModel(
            int rowCount,
            int colCount,
            int colorCount,
            IReadOnlyList<IGridItemSetting> gridItemSettings)
        {
            this.RowCount = rowCount;
            this.ColCount = colCount;
            this.ColorCount = colorCount;
            this.GridItemSettings = gridItemSettings;
        }
    }
}