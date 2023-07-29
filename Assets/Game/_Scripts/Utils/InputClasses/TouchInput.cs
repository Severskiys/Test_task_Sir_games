using System;
using UnityEngine;

namespace Utils.InputClasses
{
    public class TouchInput : MonoBehaviour
    {
        [SerializeField] private float _maxTouchShift = 50.0f;
        
        private float _currentTouchShift;

        private Vector2 _firstTouch;
        private Vector2 _inputVector;
        private bool _isOff;
        private bool _touchBegan;

        public static TouchInput Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_isOff)
                return;

            if (Input.touchCount > 0)
                ReadInputDeltaVector(Input.GetTouch(0).phase, Input.GetTouch(0).position);

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                ReadInputDeltaVector(TouchPhase.Began, Input.mousePosition);

            if (Input.GetMouseButton(0))
                ReadInputDeltaVector(TouchPhase.Moved, Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
                ReadInputDeltaVector(TouchPhase.Ended, Input.mousePosition);

#endif
        }

        public event Action<Vector2> OnTouchBegan;
        public event Action<Vector2> OnTouchStay;
        public event Action<Vector2> OnTouchStayScreenPos;
        public event Action OnTouchEnded;

        public void TurnOffInput()
        {
            _touchBegan = false;
            ReadInputDeltaVector(TouchPhase.Ended, Vector2.zero);
            _isOff = true;
        }

        public void TurnOnInput()
        {
            _isOff = false;
        }

        private void ReadInputDeltaVector(TouchPhase phase, Vector2 position)
        {
            switch (phase)
            {
                case TouchPhase.Began:
                    _firstTouch = position;
                    _touchBegan = true;
                    OnTouchBegan?.Invoke(_firstTouch);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:

                    if (_touchBegan == false)
                    {
                        _touchBegan = true;
                        _firstTouch = position;
                        OnTouchBegan?.Invoke(_firstTouch);
                    }

                    _currentTouchShift = Vector2.Distance(position, _firstTouch);
                    _inputVector = _currentTouchShift < _maxTouchShift
                        ? position
                        : Vector2.Lerp(_firstTouch, position, _maxTouchShift / _currentTouchShift);
                    OnTouchStay?.Invoke(_inputVector - _firstTouch);
                    OnTouchStayScreenPos?.Invoke(position);
                    break;

                case TouchPhase.Ended:
                    _touchBegan = false;
                    OnTouchEnded?.Invoke();
                    break;
            }
        }
    }
}