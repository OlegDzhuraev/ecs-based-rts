using System.Collections.Generic;
using InsaneOne.EcsRts.Storing;
using InsaneOne.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace InsaneOne.EcsRts
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

            ref var coloredRenderers = ref unitEntitiy.Get<ColoredRenderersComponent>();
            coloredRenderers.Renderers = unitParts.ColoredRenderers;

            ref var effects = ref unitEntitiy.Get<EffectsComponent>();
            effects.ShootEffect = data.Effects.ShootEffect;
            
            if (data.Attack.HaveTurret)
            {
                ref var turret = ref unitEntitiy.Get<TurretComponent>();
                turret.Turret = unitParts.Turret;
            }
            
            if (data.Attack.CanAttack)
            {
                ref var attack = ref unitEntitiy.Get<AttackComponent>();
                attack.Data = data.Attack;
                attack.ShootPoint = unitParts.ShootPoint;
            }

            if (data.Move.CanMove)
            {
                ref var movable = ref unitEntitiy.Get<MovableComponent>();
                
                movable.Transform = spawnedObject.transform;
                movable.Data = data.Move;
                movable.StopSqrDistance = Mathf.Pow(data.Move.StopDistance, 2);
                movable.LookInMoveDirection = true;
            
                movable.Destination = spawnedObject.transform.position;
                
                ref var navComponent = ref unitEntitiy.Get<NavMeshComponent>();
                navComponent.NavMeshAgent = spawnedObject.AddComponent<NavMeshAgent>();
                navComponent.NavMeshAgent.speed = movable.Data.MoveSpeed;
                navComponent.NavMeshAgent.destination = spawnedObject.transform.position;
                navComponent.NavMeshAgent.angularSpeed = movable.Data.RotationSpeed;
            }
            else
            {
                var obstacle = spawnedObject.AddComponent<NavMeshObstacle>(); // making not moving objects stationary obstacle in navmesh
                obstacle.carving = true;
                obstacle.size = spawnedObject.GetComponent<Collider>().bounds.size;
            }

            if (data.Production.CanProduceUnits)
            {
                ref var production = ref unitEntitiy.Get<ProductionComponent>();
                
                production.Data = data.Production;
                production.SpawnPoint = spawnedObject.transform.position + spawnedObject.transform.forward * 2f; // todo replace this hardcode
                production.Queue = new List<UnitData>();
            }

            if (data.Harvest.CanHarvest)
            {
                ref var harvester = ref unitEntitiy.Get<HarvesterComponent>();
                
                harvester.GiveResourcesPoint = spawnedObject.transform.position; // todo replace this temporary point
                harvester.Data = data.Harvest;
            }

            ref var changeOwnerEvent = ref unitEntitiy.Get<ChangeUnitOwnerEvent>();
            changeOwnerEvent.NewOwnerPlayerEntity = spawnUnitEvent.OwnerPlayer;
            changeOwnerEvent.NewOwnerPlayerId = spawnUnitEvent.OwnerPlayerId;
        }
    }
}