using InsaneOne.EcsRts.Storing;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    public struct MovableComponent
    {
        public MoveData Data;
        public Vector3 Destination;
        public Transform Transform;
      
        public float StopSqrDistance;
        
        public bool LookInMoveDirection;
    }
}