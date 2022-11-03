using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModel
    {
        public readonly int TypeId;
        public readonly Color Color;
        public readonly Sprite Sprite;

        public GridItemModel(GridItemSetting itemSetting)
        {
            this.TypeId = itemSetting.ItemId;
            this.Color = itemSetting.Color;
            this.Sprite = itemSetting.Sprite;
        }
    }
}