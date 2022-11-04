using UnityEngine;

namespace TapMatch.GridSystem
{
    public interface IGridItemSetting
    {
        public int TypeId { get; }
        public Color Color { get; }
        public Sprite Sprite { get; }
    }
}