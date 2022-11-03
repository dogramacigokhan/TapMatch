using TapMatch.GridSystem.Interactions;
using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridView : MonoBehaviour, IInteractionProvider
    {
        [SerializeField] private GridItemView gridItemViewPrefab;
        private GridViewModel viewModel;
        private GridItemView[,] gridItemViews;

        public event GridItemSelectedEventHandler GridItemSelected;

        public void Init(GridViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.viewModel.DestroyGridItems += this.OnDestroyGridItems;
        }

        private void Start()
        {
            var rowCount = this.viewModel.GridItemModels.GetLength(0);
            var colCount = this.viewModel.GridItemModels.GetLength(1);
            this.gridItemViews = new GridItemView[rowCount, colCount];

            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < colCount; j++)
                {
                    var gridItemView = Instantiate(
                        this.gridItemViewPrefab,
                        new Vector3(j - ((float)(colCount - 1) / 2), ((float)(rowCount - 1) / 2) - i, 0),
                        Quaternion.identity,
                        this.transform);

                    var itemModel = this.viewModel.GridItemModels[i, j];
                    gridItemView.Init(itemModel);
                    gridItemView.Clicked += this.OnClickedToItemView;

                    this.gridItemViews[i, j] =  gridItemView;
                }
            }
        }

        private void OnClickedToItemView(int row, int column)
        {
            this.GridItemSelected?.Invoke(row, column);
        }

        private void OnDestroyGridItems(int[] rows, int[] columns)
        {
            for (var i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                var column = columns[i];

                this.gridItemViews[row, column].DestroyView();
            }
        }
    }
}
