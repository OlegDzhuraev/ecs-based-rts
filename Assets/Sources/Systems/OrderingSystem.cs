using Leopotam.Ecs;
using Sources.Components.Tags;
using UnityEngine;

namespace Sources 
{
    sealed class OrderingSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly EcsSystems systems = null;
        readonly Camera camera;
        
        readonly EcsFilter<SelectedTag> filter = null;

        readonly RaycastHit[] raycastHits = new RaycastHit[1];
        
        void IEcsRunSystem.Run ()
        {
            if (!Input.GetMouseButtonDown(1))
                return;

            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var hitsCount = Physics.RaycastNonAlloc(ray, raycastHits, 1000);

            if (hitsCount == 0)
                return;
            
            foreach (var i in filter)
            {
                ref var moveOrderEvent = ref filter.GetEntity(i).Get<MoveOrderEvent>();
                moveOrderEvent.DestinationPosition = raycastHits[0].point;
            }
        }
    }
}