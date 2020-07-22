using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class AttackSystem : IEcsRunSystem
    {
        readonly EcsSystems ecsSystems;
        
        readonly EcsFilter<AttackComponent, UnitTargetComponent> filter = null;
        
        void IEcsRunSystem.Run ()
        {
            var dTime = Time.deltaTime;
            
            foreach (var i in filter)
            {
                ref var attackComponent = ref filter.Get1(i);
                ref var unitTarget = ref filter.Get2(i);
                var unitEntity = filter.GetEntity(i);
                
                if (attackComponent.IsReloading)
                {
                    attackComponent.ReloadTimeLeft -= dTime;
                    if (attackComponent.ReloadTimeLeft <= 0)
                        attackComponent.IsReloading = false;
                    
                    continue;
                }

                unitEntity.Get<ShootEvent>();
                
                attackComponent.IsReloading = true;
                attackComponent.ReloadTimeLeft = attackComponent.Data.ReloadTime;
                
                ref var damageEvent = ref unitTarget.EnemyTargetEntity.Get<TakeDamageEvent>();
                damageEvent.Value = attackComponent.Data.Damage;
            }
        }
    }
}