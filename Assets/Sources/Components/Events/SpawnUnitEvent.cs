using InsaneOne.EcsRts.Storing;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    struct SpawnUnitEvent
    {
        public UnitData UnitToSpawnData;
        public Vector3 Position;
        public int OwnerPlayerId;
    }
}