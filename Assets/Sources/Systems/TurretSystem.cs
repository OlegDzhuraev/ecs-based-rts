using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class TurretSystem : IEcsRunSystem 
    {
        readonly EcsFilter<TurretComponent, UnitTargetComponent> targetFilter = null;
        readonly EcsFilter<TurretComponent>.Exclude<UnitTargetComponent> noTargetFilter = null;

        const float tempTurRotSpeed = 360;
        
        void IEcsRunSystem.Run ()
        {
            var dTime = Time.deltaTime;
            
            HandleWithTargets(dTime);
            HandleNoTargets(dTime);
        }

        void HandleWithTargets(float dTime)
        {
            foreach (var i in targetFilter)
            {
                ref var turretComponent = ref targetFilter.Get1(i);
                ref var unitTargetComponent = ref targetFilter.Get2(i);

                var turret = turretComponent.Turret;

                var direction = (unitTargetComponent.EnemyTarget.Position - turret.position).normalized;
                turret.rotation = Quaternion.RotateTowards(turret.rotation, Quaternion.LookRotation(direction),
                    tempTurRotSpeed * dTime);
            }
        }
        
        void HandleNoTargets(float dTime)
        {
            foreach (var i in noTargetFilter)
            {
                ref var turretComponent = ref noTargetFilter.Get1(i);
                var turret = turretComponent.Turret;
                
                turret.localRotation = Quaternion.RotateTowards(turret.localRotation,Quaternion.identity, tempTurRotSpeed * dTime);
            }
        }
    }
}