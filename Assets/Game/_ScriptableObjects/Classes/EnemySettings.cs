using UnityEngine;
using Utils;

namespace _ScriptableObjects.Classes
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ISN/EnemySettings")]
    public class EnemySettings : CharacterSettings
    {
        [SerializeField] private Vector2 _attackTimeMinMax;
        [SerializeField] private Vector2 _targetBordersAroundPlayer;
        [SerializeField] private bool _needToSeePlayer;
        [SerializeField, Min(0)] private int _coinsForKill;
        [SerializeField] private LayerMask _layerMask;
        public float AttackTimer => _attackTimeMinMax.RandomValue();
        public Vector2 TargetBordersAroundPlayer => _targetBordersAroundPlayer;
        public bool NeedToSeePlayer => _needToSeePlayer;
        public int CoinsForKill => _coinsForKill;
        public LayerMask LayerMask => _layerMask;
    }
}