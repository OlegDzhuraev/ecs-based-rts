using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    struct UnitComponent
    {
        public GameObject SelfObject;
        public Transform Transform;
        public Vector3 Position => Transform ? Transform.position : Vector3.zero;

        public DefenseData DefenseData;
        
        public float Health;

        public int OwnerPlayerId;
        public EcsEntity OwnerPlayer;
    }
}