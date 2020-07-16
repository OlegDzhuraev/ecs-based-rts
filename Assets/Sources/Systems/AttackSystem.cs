using Leopotam.Ecs;
using Sources.Components;
using UnityEngine;

namespace Sources.Systems
{
    sealed class AttackSystem : IEcsRunSystem 
    {
        readonly EcsFilter<AttackComponent, UnitTargetComponent> filter = null;
        
        void IEcsRunSystem.Run ()
        {
            var dTime = Time.deltaTime;
            
            foreach (var i in filter)
            {
                ref var attackComponent = ref filter.Get1(i);
                ref var unitTargetComponent = ref filter.Get2(i);
       
                if (attackComponent.IsReloading)
                {
                    attackComponent.ReloadTimeLeft -= dTime;
                    if (attackComponent.ReloadTimeLeft <= 0)
                        attackComponent.IsReloading = false;
                    
                    continue;
               
                }

                DoShoot(ref attackComponent, unitTargetComponent.EnemyTargetEntity);
            }
        }

        void DoShoot(ref AttackComponent byAttackComponent, in EcsEntity toUnit)
        {
            byAttackComponent.IsReloading = true;
            byAttackComponent.ReloadTimeLeft = byAttackComponent.Data.ReloadTime;
            
            ref var damageEvent = ref toUnit.Get<TakeDamageEvent>();
            damageEvent.Value = byAttackComponent.Data.Damage;
        }
    }
}