using Leopotam.Ecs;

namespace InsaneOne.EcsRts.UI 
{
    struct ShowBuyButtonsEvent
    {
        public EcsEntity ProductionEntity;
        public ProductionComponent Production;
    }
}