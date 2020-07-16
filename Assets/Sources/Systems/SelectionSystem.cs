using Leopotam.Ecs;
using UnityEngine;
using Sources.Components;
using Sources.Components.Tags;

namespace Sources.Systems
{
    sealed class SelectionSystem : IEcsRunSystem 
    {
        // auto-injected fields
        readonly EcsWorld world = null;
        
        readonly EcsFilter<UnitComponent> unitsFilter = null;
        readonly EcsFilter<SelectedTag> selectedFilter = null;
        
        // todo replace to inject or smth
        Camera camera;

        void IEcsRunSystem.Run()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            foreach (var i in selectedFilter)
                selectedFilter.GetEntity(i).Del<SelectedTag>();

            if (!camera)
                camera = Camera.main;
            
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 1000))
            {
                foreach (var i in unitsFilter)
                {
                    var unitObject = unitsFilter.Get1(i).SelfObject;

                    if (hit.collider.gameObject == unitObject)
                    {
                        var entity = unitsFilter.GetEntity(i);

                        entity.Get<SelectedTag>();
                    }
                }
            }
        }
    }
}