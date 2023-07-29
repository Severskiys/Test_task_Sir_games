using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class Helper
    {
        #region PerformScale

        public static void PerformCuteScale(Vector3 startScale, Vector3 endScale, Transform transformToScale,
            float duration, Action doOnComplete = null, Ease ease = Ease.Linear)
        {
            transformToScale.localScale = startScale;

            if (endScale == Vector3.zero)
                transformToScale.DOScale(startScale * 1.45f, duration * 0.5f).SetEase(ease)
                    .OnComplete(() => transformToScale.DOScale(endScale, duration * 0.5f)).SetEase(ease)
                    .OnComplete(() => doOnComplete?.Invoke());
            else
                transformToScale.DOScale(endScale * 1.45f, duration * 0.5f).SetEase(ease)
                    .OnComplete(() => transformToScale.DOScale(endScale, duration * 0.5f)).SetEase(ease)
                    .OnComplete(() => doOnComplete?.Invoke());
        }

        #endregion

        #region GetRandomPointOnPlane

        public static Vector3 GetRandomPointOnPlane(Transform target, float radius)
        {
            var insideUnitCircle = Random.insideUnitCircle;
            var direction = new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y);
            return target.position + direction * radius;
        }

        #endregion

        #region ListShuffle

        public static List<T> ShuffleList<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        #endregion

        #region MoneyShrinker

        public static string MoneyString(this int moneyValue)
        {
            if (moneyValue < 0) return "0";

            if (moneyValue < 10_000) return moneyValue.ToString();

            var moneyString = moneyValue.ToString();
            var thousandCount = (moneyString.Length - 1) / 3;
            switch (thousandCount)
            {
                case 1:
                    moneyString = moneyString.Substring(0, moneyString.Length - 3) + "K";
                    break;
                case 2:
                    moneyString = moneyString.Substring(0, moneyString.Length - 6) + "M";
                    break;
                case 3:
                    moneyString = moneyString.Substring(0, moneyString.Length - 9) + "B";
                    break;
                default:
                    moneyString = moneyString.Substring(0, moneyString.Length - 9) + "B";
                    break;
            }

            return moneyString;
        }

        #endregion

        #region CameraMain

        private static Camera _camera;

        public static Camera Camera
        {
            get
            {
                if (_camera == default)
                    _camera = Camera.main;

                return _camera;
            }
        }

        #endregion

        #region WaitDictionary

        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait))
                return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        public static IEnumerator WaitCoroutine(float duration, Action finished = null)
        {
            yield return GetWait(duration);
            finished?.Invoke();
        }
        
        #endregion

        #region PointerOverUI

        private static PointerEventData _eventDataCurrentPos;
        private static List<RaycastResult> _result;

        public static bool IsPointerOverUI()
        {
            _eventDataCurrentPos = new PointerEventData(EventSystem.current)
                { position = UnityEngine.Input.mousePosition };
            _result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPos, _result);
            return _result.Count > 0;
        }

        #endregion
    }
}