using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModel
    {
        public readonly int Id;
        public readonly Color Color;
        public readonly Sprite Sprite;

        public GridItemModel(GridItemSetting itemSetting)
        {
            this.Id = itemSetting.ItemId;
            this.Color = itemSetting.Color;
            this.Sprite = itemSetting.Sprite;
        }
    }
}