using System;
using DG.Tweening;
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

        private Vector3 initialLocalScale;

        private void Awake()
        {
            this.initialLocalScale = this.transform.localScale;
        }

        private void OnEnable()
        {
            this.transform.localScale = this.initialLocalScale;
        }

        public void Init(GridItemModel model, int row, int column)
        {
            this.model = model;
            this.row = row;
            this.column = column;
            this.spriteRenderer.color = model.Color;
            this.spriteRenderer.sprite = model.Sprite;
        }

        public void ShiftRow(int amount)
        {
            this.row += amount;
        }

        public void OnMouseDown()
        {
            this.Clicked?.Invoke(this.row, this.column);
        }

        public void AnimateOut(Action onCompleteAction)
        {
            this.transform
                .DOScale(Vector3.zero, 0.2f)
                .OnComplete(() => onCompleteAction?.Invoke());
        }
    }
}