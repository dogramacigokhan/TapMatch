using TapMatch.GridSystem;
using TapMatch.GridSystem.Interactions;
using TapMatch.Networking;
using UnityEngine;

namespace TapMatch.Initialization
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private GridView gridViewPrefab;
        [SerializeField] private GridItemSetting[] gridItemSettings;

        private void Awake()
        {
            var gridView = Instantiate(this.gridViewPrefab);
            var interactionProviders = new IInteractionProvider[]
            {
                gridView,
                new NetworkInteractionSource(),
            };

            var viewModel = new GridViewModel(
                rowCount: 8,
                colCount: 5,
                this.gridItemSettings,
                interactionProviders,
                new DepthFirstGridMatchFinder());

            gridView.Init(viewModel);
        }
    }
}