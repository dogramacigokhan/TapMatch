using System;
using TapMatch.GridSystem.Interactions;

namespace TapMatch.Networking
{
    public class NetworkInteractionSource : IInteractionProvider
    {
        public event Action<(int row, int column)> GridItemSelected;
    }
}