using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class MovableSystem : IEcsRunSystem 
    {
        readonly EcsFilter<MovableComponent> filter = null;
        
        void IEcsRunSystem.Run ()
        {
            var deltaTime = Time.deltaTime;
            
            foreach (var i in filter)
            {
                ref var movable = ref filter.Get1(i);

                var rawDirectionVector = movable.Destination - movable.Transform.position;
                var direction = rawDirectionVector.normalized;
                var distance = rawDirectionVector.sqrMagnitude;
                
                if (distance <= movable.StopSqrDistance)
                    continue;
                
                movable.Transform.position += deltaTime * movable.Data.MoveSpeed * direction;
                
                if (movable.LookInMoveDirection)
                    movable.Transform.rotation = Quaternion.RotateTowards(movable.Transform.rotation, Quaternion.LookRotation(direction), movable.Data.RotationSpeed * deltaTime);
            }
        }
    }
}