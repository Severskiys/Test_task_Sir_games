using System;
using System.Collections.Generic;
using System.Linq;
using _ScriptableObjects.Classes;
using Gameplay.EnemyScripts;
using UnityEngine;

namespace Gameplay.Zones
{
    public class TargetLocator : MonoBehaviour
    {
        [SerializeField] private InteractionZone _interactionZone;
        
        private List<EnemyCharacter> _allTargetList = new();
        private List<EnemyCharacter> _closestTargets = new List<EnemyCharacter>();
        private CharacterSettings _settings;

        public bool HasTargetInRange => _allTargetList.Count > 0;

        public void Init(CharacterSettings settings)
        {
            _settings = settings;
            _interactionZone.Init(_settings.LocationRadius);
            
            _interactionZone.OnZoneEnter += OnInteractionZoneEnter;
            _interactionZone.OnZoneExit += OnInteractionZoneExit;
        }

        public EnemyCharacter GetClosestTarget(Vector3 position)
        {
            if (_allTargetList.Count != 0)
            {
                _closestTargets = _allTargetList.OrderBy(d => Vector3.Distance(d.transform.position, position)).ToList();
                return _closestTargets[0];
            }

            return null;
        }

        private void OnDisable()
        {
            _interactionZone.OnZoneEnter -= OnInteractionZoneEnter;
            _interactionZone.OnZoneExit -= OnInteractionZoneExit;

            foreach (var iDamageable in _allTargetList)
                iDamageable.OnDeath -= RemoveTarget;
        }

        private void OnInteractionZoneEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyCharacter target))
                AddTarget(target);
        }

        private void OnInteractionZoneExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyCharacter target))
                RemoveTarget(target);
        }

        private void AddTarget(EnemyCharacter target)
        {
            if (target.IsAlive == false)
                return;

            if (_allTargetList.Contains(target))
                return;

            _allTargetList.Add(target);
            target.OnDeath += RemoveTarget;
        }

        private void RemoveTarget(EnemyCharacter target)
        {
            target.OnDeath -= RemoveTarget;
            _allTargetList.Remove(target);
        }
    }
}