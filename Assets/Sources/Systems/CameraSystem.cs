using Leopotam.Ecs;
using Sources.Components;
using Sources.Storing;
using UnityEngine;

namespace Sources 
{
    sealed class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld world;
        readonly GameStartData startData;
        
        readonly EcsFilter<CameraComponent, MovableComponent> filter = null;

        Vector3 startMousePosition;
        bool mouseInputOn;
        
        public void Init()
        {
            var cameraEntity = world.NewEntity();
            ref var cameraComponent = ref cameraEntity.Get<CameraComponent>();
            ref var movable = ref cameraEntity.Get<MovableComponent>();

            cameraComponent.Camera = Camera.main;

            movable.Transform = cameraComponent.Camera.transform;
            movable.Destination = movable.Transform.position;
            movable.Data.MoveSpeed = startData.CameraSpeed;
        }
        
        void IEcsRunSystem.Run ()
        {
            var input = GetKeysFormattedInput() + GetMouseFormattedInput();
            
            foreach (var i in filter)
            {
                ref var movable = ref filter.Get2(i);
               
                movable.Destination = movable.Transform.position + input;
                movable.Data.MoveSpeed = startData.CameraSpeed * input.magnitude;
            }
        }

        Vector3 GetKeysFormattedInput() => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 GetMouseFormattedInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                startMousePosition = Input.mousePosition;
                mouseInputOn = true;
            }
            
            if (Input.GetMouseButtonUp(1))
                mouseInputOn = false;
            
            if (!mouseInputOn)
                return Vector3.zero;
            
            var screenSize = new Vector2(Screen.width, Screen.height);
            var realInput = (Input.mousePosition - startMousePosition) / screenSize;
            
            return new Vector3(realInput.x, 0, realInput.y);
        }
    }
}