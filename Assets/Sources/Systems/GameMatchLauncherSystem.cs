using InsaneOne.EcsRts.Storing;
using InsaneOne.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    public class GameMatchLauncherSystem : IEcsInitSystem
    {
        readonly EcsWorld world;
        readonly GameStartData startData;

        readonly EcsFilter<PlayerComponent> playerFilter = null;

        SpawnPoint[] spawnPoints;

        public void Init()
        {
            spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();

            SetupPlayers();
        }

        void SetupPlayers()
        {
            for (var i = 0; i < startData.StartPlayersCount; i++)
            {
                var playerEntity = world.NewEntity();
                ref var playerComponent = ref playerEntity.Get<PlayerComponent>();

                playerComponent.Id = i;
                playerComponent.Resources = startData.StartMoney;
                playerComponent.Color = startData.PlayerColors[i];
                
                SpawnPlayerUnit(playerEntity, i);
            }
        }

        void SpawnPlayerUnit(EcsEntity playerEntity, int playerId)
        {
            for (var i = 0; i < spawnPoints.Length; i++)
            {
                var point = spawnPoints[i];
                if (point.PlayerId == playerId)
                {
                    ref var spawnEvent = ref world.NewEntity().Get<SpawnUnitEvent>();

                    spawnEvent.OwnerPlayer = playerEntity;
                    spawnEvent.OwnerPlayerId = playerId;
                    
                    spawnEvent.Position = point.transform.position;
                    spawnEvent.UnitToSpawnData = startData.StartUnit;

                    GameObject.Destroy(point);
                }
            }
        }
    }
}
    