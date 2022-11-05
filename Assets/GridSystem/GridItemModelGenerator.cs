using System.Collections.Generic;
using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModelGenerator
    {
        public const int MinAllowedColorCount = 3;

        /// <summary>
        /// Generate item models with given settings and assign them in the result array.
        /// </summary>
        /// <param name="resultArray">Result array to fill.</param>
        /// <param name="gridItemSettings">Settings to use.</param>
        /// <param name="colorCount">Color count to use to generate items. Min count is 3.</param>
        public void GenerateModels(
            GridItemModel[,] resultArray,
            IReadOnlyList<IGridItemSetting> gridItemSettings,
            int colorCount)
        {
            var rowCount = resultArray.GetLength(0);
            var colCount = resultArray.GetLength(1);

            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < colCount; column++)
                {
                    resultArray[row, column] = this.GenerateModel(gridItemSettings, colorCount);
                }
            }
        }

        /// <summary>
        /// Generate item models with given settings.
        /// </summary>
        /// <param name="gridItemSettings">Settings to use.</param>
        /// <param name="colorCount">Color count to use to generate items. Min count is 3.</param>
        /// <returns></returns>
        public GridItemModel GenerateModel(IReadOnlyList<IGridItemSetting> gridItemSettings, int colorCount)
        {
            var adjustedColorCount = Mathf.Max(MinAllowedColorCount, Mathf.Min(colorCount, gridItemSettings.Count));
            return new GridItemModel(gridItemSettings[Random.Range(0, adjustedColorCount)]);
        }
    }
}