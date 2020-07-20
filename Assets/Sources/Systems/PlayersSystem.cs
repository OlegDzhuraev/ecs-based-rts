using Leopotam.Ecs;

namespace Sources 
{
    sealed class PlayersSystem : IEcsRunSystem 
    {
        readonly EcsFilter<PlayerComponent, PlayerSpendMoneyEvent> spendMoneyFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in spendMoneyFilter)
            {
                ref var player = ref spendMoneyFilter.Get1(i);
                ref var spendEvent = ref spendMoneyFilter.Get2(i);

                player.Money -= spendEvent.Value;
            }
        }
    }
}