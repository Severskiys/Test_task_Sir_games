using UnityEngine;

namespace Utils.InputClasses
{
    public class TouchInputView : MonoBehaviour
    {
        [SerializeField] private RectTransform _touchPad;
        [SerializeField] private RectTransform _touchCircle;

        private static bool _isTurnOff;
        private Vector2 _startPosition;

        private void Start()
        {
            TouchInput.Instance.OnTouchBegan += ShowTouchPad;
            TouchInput.Instance.OnTouchStay += MoveTouchPad;
            TouchInput.Instance.OnTouchEnded += HideTouchPad;
        }

        private void OnDisable()
        {
            TouchInput.Instance.OnTouchBegan -= ShowTouchPad;
            TouchInput.Instance.OnTouchStay -= MoveTouchPad;
            TouchInput.Instance.OnTouchEnded -= HideTouchPad;
        }
        
        private void ShowTouchPad(Vector2 touchPosition)
        {
            _startPosition = touchPosition;
            _touchCircle.position = touchPosition;
            _touchPad.position = touchPosition;
            _touchPad.gameObject.SetActive(true);
            _touchCircle.gameObject.SetActive(true);
        }

        private void MoveTouchPad(Vector2 touchPosition)
        {
            _touchPad.position = _startPosition + touchPosition;
        }

        private void HideTouchPad()
        {
            _touchPad.gameObject.SetActive(false);
            _touchCircle.gameObject.SetActive(false);
        }
    }
}