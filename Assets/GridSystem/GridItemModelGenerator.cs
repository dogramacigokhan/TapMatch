using UnityEngine;

namespace TapMatch.GridSystem
{
    public class GridItemModelGenerator
    {
        public void GenerateModels(GridItemModel[,] resultArray)
        {
            var rowCount = resultArray.GetLength(0);
            var colCount = resultArray.GetLength(1);

            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < colCount; j++)
                {
                    resultArray[i, j] = new GridItemModel(Random.ColorHSV(0, 1));
                }
            }
        }
    }
}