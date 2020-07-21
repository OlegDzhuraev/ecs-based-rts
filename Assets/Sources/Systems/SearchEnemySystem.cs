using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class SearchEnemySystem : IEcsRunSystem 
    {
        readonly EcsFilter<UnitComponent, AttackComponent>.Exclude<UnitTargetComponent> searchersFilter = null;
        readonly EcsFilter<UnitComponent> unitsFilter = null;
        
        readonly EcsFilter<UnitComponent, UnitTargetComponent> attackersFilter = null;
        
        const float TempSqrDist = 25;
        
        void IEcsRunSystem.Run ()
        {
            SearchEnemies();

            CheckExistingTargets();
        }

        void SearchEnemies()
        {
            foreach (var i in searchersFilter)
            {
                ref var unit = ref searchersFilter.Get1(i);

                GetEnemyForUnit(unit, searchersFilter.GetEntity(i));
            }
        }

        void GetEnemyForUnit(in UnitComponent unit, in EcsEntity unitEntity)
        {
            foreach (var i in unitsFilter)
            {
                ref var otherUnit = ref unitsFilter.Get1(i);

                if (IsEnemies(unit, otherUnit) && CanAttackByDistance(unit.Position, otherUnit.Position, TempSqrDist))
                {
                    ref var unitTarget = ref unitEntity.Get<UnitTargetComponent>();
                    
                    unitTarget.EnemyTarget = otherUnit;
                    unitTarget.EnemyTargetEntity = unitsFilter.GetEntity(i);
                    
                    break;
                }
            }
        }
        
        bool IsEnemies(in UnitComponent unitA, in UnitComponent unitB) => unitA.OwnerPlayerId != unitB.OwnerPlayerId;

        void CheckExistingTargets()
        {
            foreach (var i in attackersFilter)
            {
                ref var unit = ref attackersFilter.Get1(i);
                ref var unitTarget = ref attackersFilter.Get2(i);
                
                if (!CanAttackByDistance(unit.Position, unitTarget.EnemyTarget.Position, TempSqrDist))
                    attackersFilter.GetEntity(i).Del<UnitTargetComponent>();
            }
        }
        
        bool CanAttackByDistance(Vector3 aPos, Vector3 bPos, float sqrDist)
        {
            var distance = (aPos - bPos).sqrMagnitude;

            return distance <= sqrDist;
        }
    }
}