using UnityEngine;

namespace Utils.Pool
{
	public class PooledParticle : PoolItem
	{
		[SerializeField] private ParticleSystem _particle;
		
		public void ReleaseAfter(float delay = 0)
		{
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(Helper.WaitCoroutine(delay, () =>
				{
					_particle.Stop();
					base.Release();
				}));
			}
		}

		public void Play() => _particle.Play();
		public void Stop() => _particle.Stop();
	}
}