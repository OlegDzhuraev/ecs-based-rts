using InsaneOne.EcsRts.UI;
using Leopotam.Ecs;
using Sources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InsaneOne.EcsRts
{
    // todo requires improvement and optimization, maybe make a service?
    sealed class SelectionSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly Camera camera;
        
        readonly EcsFilter<UnitComponent> unitsFilter = null;
        readonly EcsFilter<SelectedTag, UnitComponent> selectedFilter = null;
        readonly EcsFilter<HoveredTag, UnitComponent> hoveredFilter = null;
        
        void IEcsRunSystem.Run()
        {
            var clickNotOnUi = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();

            if (clickNotOnUi)
            {
                foreach (var i in selectedFilter)
                {
                    ref var unit = ref selectedFilter.Get2(i);
                    ref var e = ref selectedFilter.GetEntity(i);
                    
                    if (!e.Has<HoveredTag>())
                        world.NewEntity().Get<RemoveHealthbarEvent>().FromUnit = unit;
                    
                    world.NewEntity().Get<HideBuyButtonsEvent>();

                    e.Del<SelectedTag>();
                }
            }
            
            foreach (var x in hoveredFilter)
            {
                ref var e = ref hoveredFilter.GetEntity(x);
                e.Del<HoveredTag>();
            }

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 1000))
            {
                foreach (var i in unitsFilter)
                {
                    ref var unitComponent = ref unitsFilter.Get1(i);
                    var unitObject = unitComponent.SelfObject;

                    if (hit.collider.gameObject == unitObject)
                    {
                        var e = unitsFilter.GetEntity(i);
                        
                        if (clickNotOnUi && unitComponent.OwnerPlayerId == PlayerComponent.LocalPlayerId)
                        {
                            e.Get<SelectedTag>();
                            
                            if (e.Has<ProductionComponent>())
                            {
                                ref var showButtonsEvent = ref world.NewEntity().Get<ShowBuyButtonsEvent>();

                                showButtonsEvent.ProductionEntity = e;
                                showButtonsEvent.Production = e.Get<ProductionComponent>();
                            }
                        }
                        
                        world.NewEntity().Get<AddHealthbarEvent>().ToUnit = unitComponent;
                        e.Get<HoveredTag>();
                    }
                }
            }

            foreach (var i in unitsFilter)
            {
                ref var unit = ref unitsFilter.Get1(i);
                ref var e = ref unitsFilter.GetEntity(i);
                
                if (!e.Has<SelectedTag>() && !e.Has<HoveredTag>())
                    world.NewEntity().Get<RemoveHealthbarEvent>().FromUnit = unit;
            }
        }
    }
}