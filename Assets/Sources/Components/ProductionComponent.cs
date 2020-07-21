using System.Collections.Generic;
using InsaneOne.EcsRts.Storing;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    struct ProductionComponent
    {
        public ProductionData Data;
        
        public List<UnitData> Queue;
        public float BuildTimer;
        public UnitData ProducingUnit;
        public Vector3 SpawnPoint;
    }
}