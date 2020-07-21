using Leopotam.Ecs;

namespace InsaneOne.EcsRts
{
    /// <summary> Компонент содержит цель юнита. Если этот компонент добавлен на энтити, значит, юнит имеет цель для атаки. </summary>
    struct UnitTargetComponent
    {
        public UnitComponent EnemyTarget;
        public EcsEntity EnemyTargetEntity;
    }
}