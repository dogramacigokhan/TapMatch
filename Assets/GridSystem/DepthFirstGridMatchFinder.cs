using System.Collections.Generic;
using System.Linq;

namespace TapMatch.GridSystem
{
    public class DepthFirstGridMatchFinder : IGridMatchFinder
    {
        public GridIndex[] FindMatches(IMatchableItem[,] matchableItems, int startRow, int startColumn)
        {
            var foundMatches = new HashSet<GridIndex>();
            var targetTypeId = matchableItems[startRow, startColumn].TypeId;

            FindMatchesRecursively(matchableItems, startRow, startColumn, targetTypeId, foundMatches);
            return foundMatches.ToArray();
        }

        private static void FindMatchesRecursively(
            IMatchableItem[,] matchableItems,
            int row,
            int column,
            int targetTypeId,
            HashSet<GridIndex> foundMatches)
        {
            if (row < 0 ||
                row >= matchableItems.GetLength(0) ||
                column < 0 ||
                column >= matchableItems.GetLength(1))
            {
                return;
            }

            if (matchableItems[row, column].TypeId != targetTypeId)
            {
                return;
            }

            var index = new GridIndex(row, column);
            if (foundMatches.Contains(index))
            {
                return;
            }

            foundMatches.Add(index);
            FindMatchesRecursively(matchableItems, row - 1, column, targetTypeId, foundMatches);
            FindMatchesRecursively(matchableItems, row, column + 1, targetTypeId, foundMatches);
            FindMatchesRecursively(matchableItems, row + 1, column, targetTypeId, foundMatches);
            FindMatchesRecursively(matchableItems, row, column - 1, targetTypeId, foundMatches);
        }
    }
}