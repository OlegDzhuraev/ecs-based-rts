using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts 
{
    sealed class ProductionSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly EcsFilter<ProductionComponent, UnitComponent> filter = null;
        readonly EcsFilter<ProductionComponent, SelectedTag> selectedFilter = null;

        readonly EcsFilter<UnitComponent, ProductionComponent, RequestBuyUnitEvent> buyRequestsFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in selectedFilter)
                DebugKeysInput(ref filter.GetEntity(i), ref filter.Get1(i));
            
            HandleBuyRequests();
            HandleQueue();
        }

        void HandleBuyRequests()
        {
            foreach (var i in buyRequestsFilter)
            {   
                ref var unit = ref buyRequestsFilter.Get1(i);
                ref var production = ref buyRequestsFilter.Get2(i);
                ref var request = ref buyRequestsFilter.Get3(i);

                var unitData = request.UnitData;

                production.Queue.Add(unitData);
            }
        }
        
        void HandleQueue()
        {
            foreach (var i in filter)
            {
                ref var production = ref filter.Get1(i);
                ref var unit = ref filter.Get2(i);

                var dTime = Time.deltaTime;

                if (production.ProducingUnit)
                {
                    production.BuildTimer -= dTime;

                    if (production.BuildTimer <= 0)
                        ProduceUnit(ref production, unit.OwnerPlayerId);
                }
                else if (!production.ProducingUnit && production.Queue.Count > 0)
                {
                    MoveQueue(ref production);
                }
            }
        }

        void ProduceUnit(ref ProductionComponent production, int ownedBy)
        {
            ref var spawnUnitEvent = ref world.NewEntity().Get<SpawnUnitEvent>();
                        
            spawnUnitEvent.Position = production.SpawnPoint;
            spawnUnitEvent.OwnerPlayerId = ownedBy;
            spawnUnitEvent.UnitToSpawnData = production.ProducingUnit;
                        
            production.ProducingUnit = null;
        }

        void MoveQueue(ref ProductionComponent production)
        {    
            production.ProducingUnit = production.Queue[0];
            production.BuildTimer = production.ProducingUnit.Production.SelfProduceTime;
            
            production.Queue.RemoveAt(0);
        }

        void DebugKeysInput(ref EcsEntity productionEnt, ref ProductionComponent production)
        {
            var unitsCount = Mathf.Min(production.Data.Units.Length, 9);

            for (int i = 1; i <= unitsCount; i++)
                if (Input.GetKeyDown(i.ToString()))
                    productionEnt.Get<RequestBuyUnitEvent>().UnitData = production.Data.Units[i - 1];
        }
    }
}