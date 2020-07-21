using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts.UI
{
    sealed class FloatingSystem : IEcsRunSystem 
    {
        readonly EcsWorld world;
        readonly EcsFilter<FloatingComponent> filter = null;
        readonly Camera camera;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in filter)
            {
                ref var floating = ref filter.Get1(i);

                var worldPosition = floating.FollowTransform.position + Vector3.up * floating.Height;
                var screenPosition =  camera.WorldToScreenPoint(worldPosition); 

                floating.SelfTransform.anchoredPosition = screenPosition;
            }
        }
    }
}