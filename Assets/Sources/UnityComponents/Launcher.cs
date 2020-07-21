using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
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
            // order is important
            systems.Add(new GameMatchLauncherSystem());
            systems.Add(new AiSystem());
            
            systems.Add(new OrderingSystem());

            systems.Add(new MovableSystem());
            systems.Add(new TurretSystem());
            systems.Add(new AttackSystem());
           
            systems.Add(new NavMeshSystem());
            systems.Add(new ProductionSystem());

            systems.Add(new SelectionSystem());
            
            systems.Add(new SpawnUnitsSystem());
            systems.Add(new ChangeUnitOwnerSystem());
            systems.Add(new SearchEnemySystem());
            systems.Add(new ProcessDamageSystem());
            systems.Add(new UnitsEffectsSystem());
            
            systems.Add(new PlayersSystem());
            systems.Add(new CameraSystem());

            systems.Add(new UI.FloatingSystem()); // todo move to UI init system?
            systems.Add(new UI.HealthbarsSystem());
            systems.Add(new UI.GameInfoSystem());
            
            systems.Inject(StartData);
            systems.Inject(Camera.main);
            systems.Inject(uiCanvas);

            systems.OneFrame<TakeDamageEvent>();
            systems.OneFrame<ChangeUnitOwnerEvent>();
            systems.OneFrame<MoveOrderEvent>();
            systems.OneFrame<SpawnUnitEvent>();
            systems.OneFrame<ShootEvent>();
            systems.OneFrame<RequestBuyUnitEvent>();
            
            systems.OneFrame<UI.RemoveHealthbarEvent>();
            systems.OneFrame<UI.AddHealthbarEvent>();
            systems.OneFrame<PlayerSpendMoneyEvent>();
            
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