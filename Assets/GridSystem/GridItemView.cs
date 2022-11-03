using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Init(Sprite sprite)
        {
            this.spriteRenderer.sprite = sprite;
        }
    }
}