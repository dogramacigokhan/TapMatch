using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModel : IMatchableItem
    {
        public int TypeId { get; }
        public readonly Color Color;
        public readonly Sprite Sprite;

        public GridItemModel(IGridItemSetting itemSetting)
        {
            this.TypeId = itemSetting.TypeId;
            this.Color = itemSetting.Color;
            this.Sprite = itemSetting.Sprite;
        }
    }
}