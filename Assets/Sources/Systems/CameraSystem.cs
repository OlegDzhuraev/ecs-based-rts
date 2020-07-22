using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts 
{
    sealed class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        const float ScreenBorderInPx = 5;
        
        readonly EcsWorld world;
        readonly GameStartData startData;
        
        readonly EcsFilter<CameraComponent, MovableComponent> filter = null;
        
        Vector3 startMousePosition;
        bool mouseInputOn;

        Vector2 screenSize, screenCenter;
        Rect borderedScreenRect, screenRect;
        
        public void Init()
        {
            var cameraEntity = world.NewEntity();
            ref var cameraComponent = ref cameraEntity.Get<CameraComponent>();
            ref var movable = ref cameraEntity.Get<MovableComponent>();

            cameraComponent.Camera = Camera.main;

            movable.Transform = cameraComponent.Camera.transform;
            movable.Destination = movable.Transform.position;
            movable.Data.MoveSpeed = startData.CameraSpeed;
            
            var startPoint = new Vector2(0 + ScreenBorderInPx, 0 + ScreenBorderInPx);
            var size = new Vector2(Screen.width - ScreenBorderInPx * 2f, Screen.height - ScreenBorderInPx * 2f);
            screenSize = new Vector2(Screen.width, Screen.height);
            screenCenter = screenSize / 2f;
            
            borderedScreenRect = new Rect(startPoint, size);
            screenRect = new Rect(Vector2.zero, screenSize);
        }
        
        void IEcsRunSystem.Run ()
        {
            var input = GetKeysFormattedInput() + GetMouseFormattedInput() + GetMouseBorderInput();
            
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
            
            var realInput =  GetOffsettedInput(Input.mousePosition, startMousePosition);
            
            return ScreenPosToCameraDirection(realInput);
        }

        Vector3 GetMouseBorderInput()
        {
            var mousePosition = Input.mousePosition;
            
            if (borderedScreenRect.Contains(mousePosition) || !screenRect.Contains(mousePosition))
                return Vector3.zero;
            
            var realInput = GetOffsettedInput(Input.mousePosition, screenCenter);
            
            return ScreenPosToCameraDirection(realInput);
        }
        
        Vector2 GetOffsettedInput(Vector2 input, Vector2 center) => (input - center) / screenSize;
        Vector3 ScreenPosToCameraDirection(Vector2 screenPos) => new Vector3(screenPos.x, 0, screenPos.y);
    }
}