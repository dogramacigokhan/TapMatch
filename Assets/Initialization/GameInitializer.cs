using TapMatch.GridSystem;
using TapMatch.GridSystem.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapMatch.Initialization
{
    public class GameInitializer : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private GridCameraController gridCameraController;
        [SerializeField] private GridView gridViewPrefab;
        [SerializeField] private GridItemSetting[] gridItemSettings;

        [Header("UI")]
        [SerializeField] private TMP_InputField rowInput;
        [SerializeField] private TMP_InputField columnInput;
        [SerializeField] private TMP_InputField colorInput;
        [SerializeField] private Button resetButton;

        private GridView gridView;

        private void Awake()
        {
            this.SetupGame(rowCount: 8, colCount: 5, colorCount: 6);

            this.resetButton.onClick.AddListener(() =>
            {
                if (!int.TryParse(this.rowInput.text, out var rowCount) || rowCount < 1)
                {
                    Debug.LogError("Invalid row value!");
                    return;
                }

                if (!int.TryParse(this.rowInput.text, out var columnCount) || columnCount < 1)
                {
                    Debug.LogError("Invalid column value!");
                    return;
                }

                if (!int.TryParse(this.colorInput.text, out var colorCount) || colorCount < GridItemModelGenerator.MinAllowedColorCount)
                {
                    Debug.LogError($"Invalid color count. Min value should be: {GridItemModelGenerator.MinAllowedColorCount}");
                    return;
                }

                this.SetupGame(rowCount, columnCount, colorCount);
            });
        }

        private void SetupGame(int rowCount, int colCount, int colorCount)
        {
            if (this.gridView != null)
            {
                // Clear the previous view
                Destroy(this.gridView.gameObject);
            }

            this.gridView = Instantiate(this.gridViewPrefab);

            var interactionProviders = new IInteractionProvider[]
            {
                this.gridView,
#if UNITY_EDITOR
                GridSystem.Interactions.Editor.EditorGridInteractionProvider.Instance,
#endif
            };

            var viewModel = new GridViewModel(
                rowCount,
                colCount,
                colorCount,
                this.gridItemSettings,
                interactionProviders,
                new DepthFirstGridMatchFinder());

            this.gridView.Init(viewModel);
            this.gridCameraController.AdjustCameraSize(rowCount, colCount);
        }
    }
}