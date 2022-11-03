using System;
using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemView : MonoBehaviour
    {
        public delegate void ClickEventHandler(int row, int column);

        [SerializeField] private SpriteRenderer spriteRenderer;
        public event ClickEventHandler Clicked;

        private GridItemModel model;

        public void Init(GridItemModel model)
        {
            this.model = model;
            this.spriteRenderer.color = model.Color;
        }

        public void OnMouseDown()
        {
            this.Clicked?.Invoke(this.model.Row, this.model.Column);
        }

        public void DestroyView()
        {
            // TODO: Animate out
            Destroy(this.gameObject);
        }
    }
}