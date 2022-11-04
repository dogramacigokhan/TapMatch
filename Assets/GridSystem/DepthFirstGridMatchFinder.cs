namespace TapMatch.GridSystem
{
    public class DepthFirstGridMatchFinder : IGridMatchFinder
    {
        public GridIndex[] FindMatches(IMatchableItem[,] matchableItems, int startRow, int startColumn)
        {
            return new GridIndex[]
            {
                new(startRow - 2, startColumn - 1),
                new(startRow - 2, startColumn),
                new(startRow - 2, startColumn + 1),
                new(startRow - 1, startColumn - 1),
                new(startRow - 1, startColumn + 1),
                new(startRow, startColumn - 1),
                new(startRow, startColumn),
                new(startRow, startColumn + 1),
            };
        }
    }
}