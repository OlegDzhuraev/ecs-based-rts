using Leopotam.Ecs;
using Sources.Components.Events;
using Sources.Storing;
using Sources.Systems;
using Sources.UI.Components.Events;
using UnityEngine;

namespace Sources.UnityComponents
{
    sealed class Launcher : MonoBehaviour
    {
        [SerializeField] GameStartData StartData;
        [SerializeField] Canvas uiCanvas;
        
        EcsWorld world;
        EcsSystems systems;

        void Start () 
        {
            world = new EcsWorld();
            systems = new EcsSystems(world);
            
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
            systems.Add(new MovableSystem());
            systems.Add(new TurretSystem());
            systems.Add(new AttackSystem());
            systems.Add(new ChangeUnitOwnerSystem());
            systems.Add(new NavMeshSystem());
            
            systems.Add(new OrderingSystem());
            systems.Add(new SelectionSystem());
            
            systems.Add(new SpawnUnitsSystem());
            systems.Add(new SearchEnemySystem());
            systems.Add(new ProcessDamageSystem());

            systems.Add(new CameraSystem());
            
            systems.Add(new GameMatchLauncherSystem());

            systems.Add(new Sources.UI.Systems.FloatingSystem()); // todo move to UI init system?
            systems.Add(new Sources.UI.Systems.HealthbarsSystem());
            
            systems.Inject(StartData);
            systems.Inject(Camera.main);
            systems.Inject(uiCanvas);

            systems.OneFrame<TakeDamageEvent>();
            systems.OneFrame<ChangeUnitOwnerEvent>();
            systems.OneFrame<MoveOrderEvent>();
            systems.OneFrame<SpawnUnitEvent>();
            
            systems.OneFrame<RemoveHealthbarEvent>();
            systems.OneFrame<AddHealthbarEvent>();
            
            systems.Init();
        }

        void Update() 
        {
            systems?.Run();
        }

        void OnDestroy()
        {
            if (systems == null)
                return;
            
            systems.Destroy();
            systems = null;
            
            world.Destroy();
            world = null;
        }
    }
}