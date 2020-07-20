using Leopotam.Ecs;
using Sources.Components;
using Sources.Components.Events;
using Sources.Components.Tags;
using UnityEngine;

namespace Sources 
{
    sealed class ProductionSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly EcsFilter<ProductionComponent, UnitComponent> filter = null;
        readonly EcsFilter<ProductionComponent, SelectedTag> selectedFilter = null;
        
        void IEcsRunSystem.Run ()
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

            foreach (var i in selectedFilter)
                DebugInput(ref filter.Get1(i));
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
        
        void DebugInput(ref ProductionComponent production)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                production.Queue.Add(production.Data.Units[0]);
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
                production.Queue.Add(production.Data.Units[1]);
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
                production.Queue.Add(production.Data.Units[2]);
        }
    }
}