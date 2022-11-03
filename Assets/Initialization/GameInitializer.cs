using TapMatch.GridSystem;
using UnityEngine;

namespace TapMatch.Initialization
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private GridView gridViewPrefab;
        [SerializeField] private GridItemSetting[] gridItemSettings;

        private void Awake()
        {
            var viewModel = new GridViewModel(5, 4, this.gridItemSettings);
            var gridView = Instantiate(this.gridViewPrefab);

            gridView.Init(viewModel);
        }
    }
}