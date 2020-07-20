using Leopotam.Ecs;
using Sources.Components;
using Sources.Components.Events;
using Sources.Components.Tags;
using Sources.Storing;
using UnityEngine;

namespace Sources 
{
    sealed class ProductionSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly EcsFilter<ProductionComponent, UnitComponent> filter = null;
        readonly EcsFilter<ProductionComponent, SelectedTag> selectedFilter = null;
        
        readonly EcsFilter<PlayerComponent> playersFilter = null;
        
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
                DebugKeysInput(ref filter.Get2(i), ref filter.Get1(i));
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

        void AddToQueue(ref UnitComponent productionUnit, ref ProductionComponent production, UnitData unit)
        {
            foreach (var i in playersFilter)
            {
                ref var player = ref playersFilter.Get1(i);
               
                if (player.Id != productionUnit.OwnerPlayerId)
                    continue;
                
                if (player.Money >= unit.Production.Price)
                {
                    playersFilter.GetEntity(i).Get<PlayerSpendMoneyEvent>().Value = unit.Production.Price;
                    production.Queue.Add(unit);
                    
                    break;
                }
            }
        }
        
        void DebugKeysInput(ref UnitComponent productionUnit, ref ProductionComponent production)
        {
            var unitsCount = Mathf.Min(production.Data.Units.Length, 9);
            
            for (int i = 1; i <= unitsCount; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                    AddToQueue(ref productionUnit, ref production, production.Data.Units[i - 1]);
            }
        }
    }
}