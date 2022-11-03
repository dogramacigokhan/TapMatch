using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private GridItemView gridItemViewPrefab;
        private GridViewModel viewModel;

        public void Init(GridViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        private void Start()
        {
            var rowCount = this.viewModel.GridItemModels.GetLength(0);
            var colCount = this.viewModel.GridItemModels.GetLength(1);

            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < colCount; j++)
                {
                    Instantiate(
                        this.gridItemViewPrefab,
                        new Vector3(j, i, 0),
                        Quaternion.identity,
                        this.transform);
                }
            }
        }
    }
}
