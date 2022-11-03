using System.Collections.Generic;
using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModelGenerator
    {
        public void GenerateModels(
            GridItemModel[,] resultArray,
            IReadOnlyList<GridItemSetting> gridItemSettings)
        {
            var rowCount = resultArray.GetLength(0);
            var colCount = resultArray.GetLength(1);

            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < colCount; column++)
                {
                    resultArray[row, column] = GenerateModel(gridItemSettings);
                }
            }
        }

        public GridItemModel GenerateModel(IReadOnlyList<GridItemSetting> gridItemSettings) => new(
            gridItemSettings[Random.Range(0, gridItemSettings.Count)]);
    }
}