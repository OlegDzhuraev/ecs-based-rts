using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts 
{
    sealed class OrderingSystem : IEcsRunSystem 
    {
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