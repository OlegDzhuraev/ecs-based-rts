using Leopotam.Ecs;

namespace InsaneOne.EcsRts 
{
    sealed class PlayersSystem : IEcsRunSystem 
    {
        readonly EcsFilter<PlayerComponent, SpendPlayerResourcesEvent> spendMoneyFilter = null;
        readonly EcsFilter<PlayerComponent, AddPlayerResourcesEvent> addMoneyFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in spendMoneyFilter)
            {
                ref var player = ref spendMoneyFilter.Get1(i);
                ref var spendEvent = ref spendMoneyFilter.Get2(i);

                player.Resources -= spendEvent.Value;
            }
            
            foreach (var i in addMoneyFilter)
            {
                ref var player = ref addMoneyFilter.Get1(i);
                ref var addEvent = ref addMoneyFilter.Get2(i);

                player.Resources += addEvent.Value;
            }
        }
    }
}