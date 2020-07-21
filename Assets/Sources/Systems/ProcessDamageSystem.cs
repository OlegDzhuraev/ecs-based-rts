using InsaneOne.EcsRts.UI;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class ProcessDamageSystem : IEcsRunSystem
    {
        readonly EcsWorld world;
        readonly EcsFilter<UnitComponent, TakeDamageEvent> filter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in filter)
            {
                var unitEntity = filter.GetEntity(i);
                ref var unitComponent = ref filter.Get1(i);
                ref var takeDamageEvent = ref filter.Get2(i);
                
                TakeDamage(unitEntity, ref unitComponent, takeDamageEvent.Value);
            }
        }

        void TakeDamage(EcsEntity unitEntity, ref UnitComponent unit, in float damage)
        {
            unit.Health = Mathf.Clamp(unit.Health - damage, 0, unit.DefenseData.MaxHealth);

            if (unit.Health <= 0)
                KillUnit(unitEntity, ref unit);
        }

        void KillUnit(EcsEntity unitEntity, ref UnitComponent unit)  // todo replace to event and process death system
        {
            world.NewEntity().Get<RemoveHealthbarEvent>().FromUnit = unit;
            
            GameObject.Destroy(unit.SelfObject);

            unitEntity.Destroy();
        }
    }
}