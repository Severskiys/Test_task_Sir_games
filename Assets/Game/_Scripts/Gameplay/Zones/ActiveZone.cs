using System;
using Gameplay.PlayerScripts;
using UnityEngine;

namespace Gameplay.Zones
{
	public class ActiveZone : MonoBehaviour
	{
		public event Action OnPlayerEnter;

		[SerializeField] private ParticleSystem _particleSystem;

		private bool _isActive = true;
		
		public void DisableZone()
		{
			_particleSystem.Stop();
			_isActive = false;
		}

		public void EnableZone()
		{
			_particleSystem.Play();
			_isActive = true;
		}

		protected void OnTriggerEnter(Collider other)
		{
			if (_isActive == false)
				return;

			if (other.TryGetComponent(out Player player) == false) 
				return;
			
			OnPlayerEnter?.Invoke();
		}
	}
}