namespace TapMatch.GridSystem
{
    public interface IGridMatchFinder
    {
        /// <summary>
        /// Finds and returns matches including the start row and column.
        /// </summary>
        /// <param name="matchableItems">Grid to match items.</param>
        /// <param name="startRow">Start row.</param>
        /// <param name="startColumn">Start column.</param>
        /// <returns></returns>
        public (int row, int column)[] FindMatches(
            IMatchableItem[,] matchableItems,
            int startRow,
            int startColumn);
    }
}