using UnityEngine;

namespace TapMatch.GridSystem
{
    [CreateAssetMenu(menuName = "TapMatch/Grid Item Setting", fileName = "GridItem")]
    public class GridItemSetting : ScriptableObject
    {
        [SerializeField] private int itemId;
        [SerializeField] private Color color;
        [SerializeField] private Sprite sprite;

        public int ItemId => this.itemId;
        public Color Color => this.color;
        public Sprite Sprite => this.sprite;
    }
}