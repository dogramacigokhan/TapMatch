using UnityEditor;
using UnityEngine;

namespace TapMatch.GridSystem.Interactions.Editor
{
	public class EditorGridController : EditorWindow
	{
		[MenuItem("Tools/Game Controller")]
		private static void Init() => GetWindow<EditorGridController>().Show();

		private int row;
		private int column;

		private void OnGUI()
		{
			if (!Application.isPlaying)
			{
				EditorGUILayout.LabelField("Only available in play mode.");
				return;
			}

			this.row = EditorGUILayout.IntField("Row: ", this.row);
			this.column = EditorGUILayout.IntField("Column: ", this.column);

			if (GUILayout.Button("Select"))
			{
				if (this.row < 0 || this.column < 0)
				{
					Debug.LogError("Invalid row or column values.");
					return;
				}

				EditorGridInteractionProvider.Instance.PublishValue(this.row, this.column);
			}
		}
	}
}