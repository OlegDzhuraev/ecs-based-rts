using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.EcsRts.UI
{
    sealed class HealthbarsSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly GameStartData startData = null;
        readonly Canvas canvas = null;
        
        readonly EcsFilter<HealthbarComponent> filter = null;
        readonly EcsFilter<UnitComponent, TakeDamageEvent> takeDamageFilter = null;
        readonly EcsFilter<AddHealthbarEvent> addHealthbarFilter = null;
        readonly EcsFilter<RemoveHealthbarEvent> removeHealthbarFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var q in takeDamageFilter)
            {
                ref var unit = ref takeDamageFilter.Get1(q);
                
                foreach (var w in filter)
                {
                    ref var healthbar = ref filter.Get1(w);

                    if (healthbar.UnitComponent.SelfObject == unit.SelfObject)
                    {
                        healthbar.FillImage.fillAmount = GetPercents(unit.Health, unit.DefenseData.MaxHealth);
                        
                        // todo colorize healthbar depending on HP value
                    }
                }
            }

            foreach (var i in addHealthbarFilter)
                CreateHealthbarEntity(ref addHealthbarFilter.Get1(i).ToUnit);

            foreach (var i in removeHealthbarFilter)
            {
                ref var removeEvent = ref removeHealthbarFilter.Get1(i);

                foreach (var w in filter)
                {
                    ref var healthbar = ref filter.Get1(w);
                    
                    if (removeEvent.FromUnit.SelfObject == healthbar.UnitComponent.SelfObject)
                    {
                        GameObject.Destroy(healthbar.SelfObject); // todo pool?
                        
                        filter.GetEntity(w).Destroy();
                        break;
                    }
                }
            }
        }
        
        void CreateHealthbarEntity(ref UnitComponent forUnit)
        {
            var healthbarEntity = world.NewEntity();
            ref var healthbar = ref healthbarEntity.Get<HealthbarComponent>();
            ref var floating = ref healthbarEntity.Get<FloatingComponent>();

            var spawnedObject = GameObject.Instantiate(startData.HealthbarTemplate, canvas.transform); // todo pool?
            var rectTransform = spawnedObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = rectTransform.anchorMax = Vector3.zero;
            
            healthbar.UnitComponent = forUnit;
            healthbar.SelfObject = spawnedObject;
            healthbar.FillImage = spawnedObject.transform.Find("Fill").GetComponent<Image>();
            healthbar.FillImage.fillAmount = GetPercents(forUnit.Health, forUnit.DefenseData.MaxHealth);
            
            floating.Height = 1f;
            floating.FollowTransform = forUnit.Transform;
            floating.SelfTransform = rectTransform;
        }

        float GetPercents(float value, float maxValue) => value / maxValue;
    }
}