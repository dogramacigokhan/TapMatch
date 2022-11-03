using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModel
    {
        public readonly int TypeId;
        public readonly int Row;
        public readonly int Column;
        public readonly Color Color;
        public readonly Sprite Sprite;

        public GridItemModel(GridItemSetting itemSetting, int row, int column)
        {
            this.TypeId = itemSetting.ItemId;
            this.Row = row;
            this.Column = column;
            this.Color = itemSetting.Color;
            this.Sprite = itemSetting.Sprite;
        }
    }
}