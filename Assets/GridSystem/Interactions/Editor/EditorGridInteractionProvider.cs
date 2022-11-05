using System;

namespace TapMatch.GridSystem.Interactions.Editor
{
	public class EditorGridInteractionProvider : IInteractionProvider
	{
		public event Action<(int row, int column)> GridItemSelected;

		private static readonly Lazy<EditorGridInteractionProvider> LazyInstance = new(() => new EditorGridInteractionProvider());
		public static EditorGridInteractionProvider Instance => LazyInstance.Value;

		public void PublishValue(int row, int column)
		{
			this.GridItemSelected?.Invoke((row, column));
		}
	}
}