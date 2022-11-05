using System;

namespace TapMatch.GridSystem.Interactions.Editor
{
	public class EditorGridInteractionProvider : IInteractionProvider
	{
		public event Action<(int row, int column)> GridItemSelected;

		private static Lazy<EditorGridInteractionProvider> lazyInstance;
		public static EditorGridInteractionProvider Instance => lazyInstance.Value;

		static EditorGridInteractionProvider()
		{
			CreateInstance();
		}

		public void PublishValue(int row, int column)
		{
			this.GridItemSelected?.Invoke((row, column));
		}

		/// <summary>
		/// When "Enter Playmode Options" are enabled domain may not be re-loaded until the scripts are recompiled.
		/// Entry point of the app may want to re-initialize the instance to dispose unused reference.
		/// Use with caution.
		/// </summary>
		public static EditorGridInteractionProvider GetInstanceWithForcedReinitialization()
		{
			CreateInstance();
			return Instance;
		}

		private static void CreateInstance() => lazyInstance = new Lazy<EditorGridInteractionProvider>(() => new EditorGridInteractionProvider());
	}
}