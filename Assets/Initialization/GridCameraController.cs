using DG.Tweening;
using UnityEngine;

namespace TapMatch.Initialization
{
	public class GridCameraController :  MonoBehaviour
	{
		private const float minOrtographicSize = 5;
		private const float maxOrtographicSize = 19;
		private const float minGridLength = 5;
		private const float maxGridLength = 20;

		[SerializeField] private Camera targetCamera;
		private Tween sizeTween;

		public void AdjustCameraSize(int rowCount, int columnCount)
		{
			var maxDimension = Mathf.Max(rowCount, columnCount);
			var normalizedLength = Mathf.Clamp01((maxDimension - minGridLength) / (maxGridLength - minGridLength));

			var targetSize = Mathf.Lerp(minOrtographicSize, maxOrtographicSize, normalizedLength);

			this.sizeTween?.Kill(complete: true);
			this.sizeTween = this.targetCamera.DOOrthoSize(targetSize, 0.3f);
		}

		private void OnDisable()
		{
			this.sizeTween?.Kill(complete: true);
		}
	}
}