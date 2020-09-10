using Leopotam.Ecs;

namespace InsaneOne.EcsRts 
{
    sealed class NavMeshSystem : IEcsRunSystem 
    {
        readonly EcsWorld world;
        readonly EcsFilter<MovableComponent, MoveOrderEvent> ordersFilter = null;
        readonly EcsFilter<NavMeshComponent, MovableComponent> moveFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            HandleOrders();
            HandleMove();
        }

        void HandleOrders()
        {
            foreach (var i in ordersFilter)
            {
                ref var movableComponent = ref ordersFilter.Get1(i);
                ref var moveOrderEvent = ref ordersFilter.Get2(i);
                
                movableComponent.Destination = moveOrderEvent.DestinationPosition;
            }
        }

        void HandleMove()
        {
            foreach (var i in moveFilter)
            {
                ref var navMeshComponent = ref moveFilter.Get1(i);
                ref var movableComponent = ref moveFilter.Get2(i);

                var direction = (movableComponent.Destination - movableComponent.Transform.position).normalized;
                navMeshComponent.NavMeshAgent.velocity = direction;
            }
        }
    }
}