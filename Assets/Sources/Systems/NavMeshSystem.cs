using Leopotam.Ecs;
using Sources.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Sources 
{
    sealed class NavMeshSystem : IEcsRunSystem 
    {
        readonly EcsWorld world;
        readonly EcsFilter<NavMeshComponent, MovableComponent, MoveOrderEvent> ordersFilter = null;
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
                ref var navMeshComponent = ref ordersFilter.Get1(i);
                ref var movableComponent = ref ordersFilter.Get2(i);
                ref var moveOrderEvent = ref ordersFilter.Get3(i);

                var path = new NavMeshPath();

                var isGenerated = NavMesh.CalculatePath(movableComponent.Transform.position,
                    moveOrderEvent.DestinationPosition, NavMesh.AllAreas, path);

                if (isGenerated)
                {
                    navMeshComponent.Path = path;
                    navMeshComponent.CurrentPoint = 0;
                }
            }
        }

        void HandleMove()
        {
            foreach (var i in moveFilter)
            {
                ref var navMeshComponent = ref ordersFilter.Get1(i);
                ref var movableComponent = ref ordersFilter.Get2(i);

                if (navMeshComponent.Path == null ||
                    navMeshComponent.CurrentPoint >= navMeshComponent.Path.corners.Length)
                    continue;

                var pointPosition = navMeshComponent.Path.corners[navMeshComponent.CurrentPoint];
                movableComponent.Destination = pointPosition;

                if (Vector3.Distance(movableComponent.Transform.position, pointPosition) < 0.75f)
                    navMeshComponent.CurrentPoint++;
            }
        }
    }
}