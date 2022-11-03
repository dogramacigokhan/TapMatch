using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemView : MonoBehaviour
    {
        public delegate void ClickEventHandler(int row, int column);

        [SerializeField] private SpriteRenderer spriteRenderer;
        public event ClickEventHandler Clicked;

        private int row;
        private int column;
        private GridItemModel model;

        public void Init(GridItemModel model, int row, int column)
        {
            this.model = model;
            this.row = row;
            this.column = column;
            this.spriteRenderer.color = model.Color;
        }

        public void ShiftRow(int amount)
        {
            this.row += amount;
        }

        public void OnMouseDown()
        {
            this.Clicked?.Invoke(this.row, this.column);
        }

        public void DestroyView()
        {
            // TODO: Animate out
            Destroy(this.gameObject);
        }
    }
}