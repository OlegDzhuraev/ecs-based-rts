using Sources.Storing;
using UnityEngine;

namespace Sources.Components
{
    struct UnitComponent
    {
        public GameObject SelfObject;
        public Transform Transform;
        public Vector3 Position => Transform.position;

        public DefenseData DefenseData;
        
        public float Health;

        public int OwnerPlayerId;
    }
}