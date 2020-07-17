using Leopotam.Ecs;
using UnityEngine;
using Sources.Components;
using Sources.Components.Events;
using Sources.UnityComponents;

namespace Sources.Systems
{
    public class SpawnUnitsSystem : IEcsRunSystem
    {
        readonly EcsWorld world;
        readonly EcsFilter<SpawnUnitEvent> filter = null;
        
        public void Run()
        {
            foreach (var i in filter)
            {
                ref var spawnUnitEvent = ref filter.Get1(i);
                
                ProcessSpawn(spawnUnitEvent);
            }
        }

        void ProcessSpawn(in SpawnUnitEvent spawnUnitEvent)
        {
            var data = spawnUnitEvent.UnitToSpawnData;

            var spawnedObject = GameObject.Instantiate(data.Prefab, spawnUnitEvent.Position, Quaternion.identity);
            var unitParts = spawnedObject.GetComponent<UnitParts>();

            var unitEntitiy = world.NewEntity();
            
            ref var unit = ref unitEntitiy.Get<UnitComponent>();
            unit.SelfObject = spawnedObject;
            unit.Transform = spawnedObject.transform;
            unit.DefenseData = data.Defense;
            unit.Health = unit.DefenseData.MaxHealth;
            
            ref var turret = ref unitEntitiy.Get<TurretComponent>();
            turret.Turret = unitParts.Turret;
            
            ref var coloredRenderers = ref unitEntitiy.Get<ColoredRenderers>();
            coloredRenderers.Renderers = unitParts.ColoredRenderers;
            
            if (data.Attack.CanAttack)
            {
                ref var attack = ref unitEntitiy.Get<AttackComponent>();
                attack.Data = data.Attack;
            }

            if (data.Move.CanMove)
            {
                ref var movable = ref unitEntitiy.Get<MovableComponent>();
                
                movable.Transform = spawnedObject.transform;
                movable.Data = data.Move;
                movable.StopSqrDistance = Mathf.Pow(data.Move.StopDistance, 2);
                movable.LookInMoveDirection = true;
            
                movable.Destination = spawnedObject.transform.position;
                
                unitEntitiy.Get<NavMeshComponent>();
            }
            
            unitEntitiy.Get<ChangeUnitOwnerEvent>().NewOwnerPlayerId = spawnUnitEvent.OwnerPlayerId;
        }
    }
}