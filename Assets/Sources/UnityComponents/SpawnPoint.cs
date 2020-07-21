using InsaneOne.EcsRts.Storing;
using UnityEngine;

namespace InsaneOne.UnityComponents
{
    public class SpawnPoint : MonoBehaviour
    {
        [Range(0, 4)] public int PlayerId;
        
        [Header("Visualization")]
        public GameStartData GameStartData;
        
        void OnDrawGizmos()
        {
            Gizmos.color = GameStartData && GameStartData.PlayerColors.Count > PlayerId ? GameStartData.PlayerColors[PlayerId] : Color.gray;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}