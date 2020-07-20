using Leopotam.Ecs;
using Sources.Components;
using Sources.Components.Events;
using UnityEngine;

namespace Sources.Systems
{
    sealed class UnitsEffectsSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly EcsFilter<AttackComponent, EffectsComponent, ShootEvent> shootFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in shootFilter)
            {
                ref var attack = ref shootFilter.Get1(i);
                ref var effects = ref shootFilter.Get2(i);
                
                GameObject.Instantiate(effects.ShootEffect, attack.ShootPoint.position, attack.ShootPoint.rotation);
            }
        }
    }
}