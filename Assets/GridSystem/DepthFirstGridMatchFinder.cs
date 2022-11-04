namespace TapMatch.GridSystem
{
    public class DepthFirstGridMatchFinder : IGridMatchFinder
    {
        public (int row, int column)[] FindMatches(IMatchableItem[,] matchableItems, int startRow, int startColumn)
        {
            return new (int row, int column)[]
            {
                (startRow - 2, startColumn - 1),
                (startRow - 2, startColumn),
                (startRow - 2, startColumn + 1),
                (startRow - 1, startColumn - 1),
                (startRow - 1, startColumn + 1),
                (startRow, startColumn - 1),
                (startRow, startColumn),
                (startRow, startColumn + 1),
            };
        }
    }
}