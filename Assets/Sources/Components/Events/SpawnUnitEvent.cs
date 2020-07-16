using UnityEngine;
using Sources.Storing;

namespace Sources.Components.Events
{
    struct SpawnUnitEvent
    {
        public UnitData UnitToSpawnData;
        public Vector3 Position;
        public int OwnerPlayerId;
    }
}