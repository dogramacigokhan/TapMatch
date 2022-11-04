using UnityEngine;

namespace TapMatch.GridSystem
{
    [CreateAssetMenu(menuName = "TapMatch/Grid Item Setting", fileName = "GridItem")]
    public class GridItemSetting : ScriptableObject, IGridItemSetting
    {
        [SerializeField] private int typeId;
        [SerializeField] private Color color;
        [SerializeField] private Sprite sprite;

        public int TypeId => this.typeId;
        public Color Color => this.color;
        public Sprite Sprite => this.sprite;
    }
}