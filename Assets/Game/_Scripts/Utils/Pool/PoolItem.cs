using UnityEngine;

namespace Utils.Pool
{
	[SelectionBase]
	public class PoolItem : MonoBehaviour, IPoolable
	{
		public int ID { get; set; }
		public virtual void Release()
		{
			StopAllCoroutines();
			Pool.Release(ID, this);
		}

		public void SetParent(Transform parent) => transform.SetParent(parent);
		public void SetPosition(Vector3 position) => transform.position = position;
		public void SetActive(bool active) => gameObject.SetActive(active);
	}
}