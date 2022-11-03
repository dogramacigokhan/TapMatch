using TapMatch.GridSystem;
using UnityEngine;

namespace TapMatch.Initialization
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private GridView gridViewPrefab;

        private void Awake()
        {
            var viewModel = new GridViewModel(5, 4);
            var gridView = Instantiate(this.gridViewPrefab);

            gridView.Init(viewModel);
        }
    }
}