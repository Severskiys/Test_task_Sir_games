using UnityEngine;
using Utils;

namespace Gameplay.EnemyScripts
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private float _xSize;
        [SerializeField] private float _zSize;
        
        public Vector3 GetPoint()
        {
            return transform.position.GenerateRandomPosition(_xSize, _zSize);
        }

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            Vector3 pos1 = position + new Vector3(_xSize * 0.5f, 0 , _zSize * 0.5f);
            Vector3 pos2 = position + new Vector3(-_xSize * 0.5f, 0 , _zSize * 0.5f);
            Vector3 pos3 = position + new Vector3(-_xSize * 0.5f, 0 , -_zSize * 0.5f);
            Vector3 pos4 = position + new Vector3(_xSize * 0.5f, 0 , -_zSize * 0.5f);
                
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawLine(pos2, pos3);
            Gizmos.DrawLine(pos3, pos4);
            Gizmos.DrawLine(pos4, pos1);
        }
    }
}