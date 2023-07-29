using _ScriptableObjects.Classes;
using UnityEngine;
using UnityEngine.AI;
using Utils.InputClasses;

namespace Gameplay.PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        
        private Vector3 _lastPos;
        private Vector3 _direction = Vector3.zero;
        private Transform _selfTransform;
        private float _speed;
        private CharacterSettings _characterSettings;
        private bool _isSlowDown;
        private CharacterView _characterView;

        public bool IsStopped { get; private set; }

        public void Init(CharacterSettings settings, CharacterView characterView)
        {
            _characterView = characterView;
            _characterSettings = settings;
            _agent.enabled = true;
        }
        
        private void Start()
        {
            _selfTransform = transform;
            _agent.enabled = false;
            
            TouchInput.Instance.OnTouchBegan += ProcessStartMove;
            TouchInput.Instance.OnTouchStay += TakeInputVector;
            TouchInput.Instance.OnTouchEnded += SetZeroDirection;
        }

        private void OnDisable()
        {
            TouchInput.Instance.OnTouchBegan -= ProcessStartMove;
            TouchInput.Instance.OnTouchStay -= TakeInputVector;
            TouchInput.Instance.OnTouchEnded -= SetZeroDirection;
        }

        private void ProcessStartMove(Vector2 direction) => IsStopped = false;
        
        private void TakeInputVector(Vector2 inputVector) => _direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        private void SetZeroDirection()
        {
            _direction = Vector3.zero;
            IsStopped = true;
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            if (_agent.enabled == false) 
                return;
            
            Vector3 position = _selfTransform.position;
            _speed = Mathf.Lerp(_speed, (position - _lastPos).magnitude / Time.deltaTime, 0.75f);
            _lastPos = position;
            Vector3 offset = _direction * (CalculateSpeed() * Time.deltaTime);
            _agent.Move(offset);
            _speed = Mathf.Lerp(_speed, offset.magnitude * 5f, 0.25f);
            _characterView.AnimateWalk(_speed);
        }
        
        private void Rotate()
        {
            if (_direction == Vector3.zero)
                return;
            
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            var deltaRotation = Quaternion.Lerp(_selfTransform.rotation, targetRotation, 0.5f);
            _selfTransform.rotation = deltaRotation;
        }

        private int CalculateSpeed() => _characterSettings.Speed;
    }
}