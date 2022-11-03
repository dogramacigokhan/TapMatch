using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Init(GridItemModel model)
        {
            this.spriteRenderer.color = model.Color;
        }
    }
}