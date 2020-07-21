using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.EcsRts.UI
{
    sealed class GameInfoSystem : IEcsInitSystem, IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        
        readonly EcsFilter<MoneyTextComponent> filter = null;
        readonly EcsFilter<PlayerComponent, PlayerSpendMoneyEvent> spendMoneyFilter = null;
        readonly EcsFilter<PlayerComponent> playersFilter = null;
        
        void IEcsInitSystem.Init()
        {
            var moneyTextObject = GameObject.FindWithTag("UI/MoneyText");

            ref var moneyText = ref world.NewEntity().Get<MoneyTextComponent>();
            moneyText.Text = moneyTextObject.GetComponent<Text>();
            
            foreach (var i in playersFilter)
            {
                ref var player = ref playersFilter.Get1(i);
                
                if (player.Id == PlayerComponent.LocalPlayerId)
                {
                    moneyText.PlayerId = player.Id;
                    SetText(ref moneyText, player.Money);

                    break;
                }
            }
        }
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in spendMoneyFilter)
            {
                ref var player = ref spendMoneyFilter.Get1(i);

                foreach (var w in filter)
                {
                    ref var moneyText = ref filter.Get1(w);
                    
                    if (player.Id == moneyText.PlayerId)
                        SetText(ref moneyText, player.Money);
                }
            }
        }

        void SetText(ref MoneyTextComponent money, int value) => money.Text.text = "Money: $" + value;
    }
}