using System;
using UnityEngine;

namespace Gameplay.Zones
{
    [RequireComponent(typeof(SphereCollider))]
    public class InteractionZone : MonoBehaviour
    {
        public event Action<Collider> OnZoneEnter;
        public event Action<Collider> OnZoneExit;
        
        [SerializeField] private SphereCollider _collider;
        
        private void OnTriggerEnter(Collider other) => OnZoneEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) => OnZoneExit?.Invoke(other);

        public void Init(float radius) => _collider.radius = radius;
    }
}