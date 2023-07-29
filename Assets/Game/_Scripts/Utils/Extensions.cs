using System;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Random;

namespace Utils
{
	public static class Extensions
	{
		public static float RandomValue(this Vector2Int vector) => Range((float) vector.x, vector.y);
		public static float RandomValue(this Vector2 vector) => Range(vector.x, vector.y);
		
		public static void HorizontalSoftLookAt(this Transform transform, Transform target, float rotationSpeed = 5) =>
			HorizontalSoftLookAt(transform, target.position, rotationSpeed);
		
		public static void HorizontalSoftLookAt(this Transform transform, Vector3 target, float rotationSpeed = 5)
		{
			var position = transform.position;
			target.y = position.y;
			var lookVector = target - position;
			var rotation   = Quaternion.LookRotation(lookVector);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
		}
		
		public static Vector3 GenerateRandomPosition(this Vector3 center, float borderX, float borderZ)
		{
			return new Vector3(
				center.x + Range(-borderX / 2, borderX / 2),
				center.y,
				center.z + Range(-borderZ / 2, borderZ / 2));
		}
		
		public static void Show(this CanvasGroup canvasGroup, float duration = 0, float delay = 0, Action callback = null)
		{
			if (duration < 0) throw new ArgumentException("Value cannot be negative", nameof(duration));

			if (delay < 0) throw new ArgumentException("Value cannot be negative", nameof(delay));

			canvasGroup.DOKill();
			if (duration == 0 && delay == 0) canvasGroup.alpha = 1;
			canvasGroup.DOFade(1, duration).SetDelay(delay).SetLink(canvasGroup.gameObject).OnComplete(() =>
			{
				canvasGroup.interactable = true;
				canvasGroup.blocksRaycasts = true;
				callback?.Invoke();
			});
		}
		
		public static void Hide(this CanvasGroup canvasGroup, float duration = 0, float delay = 0, Action callback = null)
		{
			if (duration < 0) throw new ArgumentException("Value cannot be negative", nameof(duration));

			if (delay < 0) throw new ArgumentException("Value cannot be negative", nameof(delay));

			canvasGroup.DOKill();
			if (duration == 0 && delay == 0) canvasGroup.alpha = 0;
			canvasGroup.DOFade(0, duration).SetDelay(delay).SetLink(canvasGroup.gameObject).OnStart(() =>
			{
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
			}).OnComplete(() =>
			{
				callback?.Invoke();
			});
		}
	}
}