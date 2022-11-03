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

            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < colCount; j++)
                {
                    resultArray[i, j] = new GridItemModel(
                        gridItemSettings[Random.Range(0, gridItemSettings.Count)],
                        row: i,
                        column: j);
                }
            }
        }
    }
}