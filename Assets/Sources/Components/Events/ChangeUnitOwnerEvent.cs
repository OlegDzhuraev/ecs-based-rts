using Leopotam.Ecs;

namespace InsaneOne.EcsRts
{
    struct ChangeUnitOwnerEvent
    {
        public int NewOwnerPlayerId;
        public EcsEntity NewOwnerPlayerEntity;
    }
}