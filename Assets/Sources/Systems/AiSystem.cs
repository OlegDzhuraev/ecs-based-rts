using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class AiSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;

        readonly EcsFilter<ProductionComponent>.Exclude<LocalPlayerOwnedTag> productionFilter = null;
        readonly EcsFilter<UnitComponent, AttackComponent>.Exclude<MoveOrderEvent, LocalPlayerOwnedTag> attackersFilter = null;
        readonly EcsFilter<UnitComponent, LocalPlayerOwnedTag> allPlayerUnitsFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            HandleUnitsBuying();
            HandleUnitsControls();
        }

        void HandleUnitsBuying()
        {
            foreach (var i in productionFilter)
            {
                var entity = productionFilter.GetEntity(i);

                ref var production = ref productionFilter.Get1(i);

                if (production.Queue.Count >= 3)
                    continue;

                var units = production.Data.Units;
                entity.Get<RequestBuyUnitEvent>().UnitData = units[Random.Range(0, units.Length)];
            }
        }

        void HandleUnitsControls()
        {
            foreach (var i in attackersFilter)
            {
                var entity = attackersFilter.GetEntity(i);
                var unit = attackersFilter.Get1(i);

                ref var moveOrder = ref entity.Get<MoveOrderEvent>();
                
                var randomedId = Random.Range(0, allPlayerUnitsFilter.GetEntitiesCount());
                
                moveOrder.DestinationPosition = allPlayerUnitsFilter.Get1(randomedId).Position;
            }
        }
    }
}