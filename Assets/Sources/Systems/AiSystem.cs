using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class AiSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;

        readonly EcsFilter<ProductionComponent>.Exclude<LocalPlayerOwnedTag> productionFilter = null;
        readonly EcsFilter<UnitComponent, AttackComponent, UnitAiComponent>.Exclude<LocalPlayerOwnedTag> attackersFilter = null;
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
                ref var aiComponent = ref attackersFilter.Get3(i);

                if (!aiComponent.Target)
                {
                    var randomedId = Random.Range(0, allPlayerUnitsFilter.GetEntitiesCount());
                    
                    var target = allPlayerUnitsFilter.Get1(randomedId);
                    aiComponent.Target = target.SelfObject;
                    
                    ref var moveOrder = ref attackersFilter.GetEntity(i).Get<MoveOrderEvent>();
                    moveOrder.DestinationPosition = target.Position;
                }
            }
        }
    }
}