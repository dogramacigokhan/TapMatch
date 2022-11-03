namespace TapMatch.GridSystem.Interactions
{
    public delegate void GridItemSelectedEventHandler(int row, int column);

    public interface IInteractionProvider
    {
        public event GridItemSelectedEventHandler GridItemSelected;
    }
}