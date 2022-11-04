using System;

namespace TapMatch.GridSystem.Interactions
{
    public interface IInteractionProvider
    {
        public event Action<(int row, int column)> GridItemSelected;
    }
}