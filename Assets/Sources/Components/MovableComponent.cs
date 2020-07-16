using Sources.Storing;
using UnityEngine;

namespace Sources.Components
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