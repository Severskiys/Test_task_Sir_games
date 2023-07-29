using UnityEngine;

namespace Utils.Pool
{
    public interface IPoolable
    {
        public int ID { get; set; }
        public void Release();
        public void SetParent(Transform parent);
        public void SetActive(bool active);
    }
}