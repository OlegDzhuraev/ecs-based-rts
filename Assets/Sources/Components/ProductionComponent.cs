using System.Collections.Generic;
using Sources.Storing;
using UnityEngine;

namespace Sources 
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