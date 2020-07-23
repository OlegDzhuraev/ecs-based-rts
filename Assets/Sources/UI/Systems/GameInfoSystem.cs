using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.EcsRts.UI
{
    sealed class GameInfoSystem : IEcsInitSystem, IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        
        readonly EcsFilter<MoneyTextComponent> filter = null;
        readonly EcsFilter<PlayerComponent, SpendPlayerResourcesEvent> spendMoneyFilter = null;
        readonly EcsFilter<PlayerComponent, AddPlayerResourcesEvent> addMoneyFilter = null;
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
                    SetText(ref moneyText, player.Resources);

                    break;
                }
            }
        }
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in spendMoneyFilter)
            {
                ref var player = ref spendMoneyFilter.Get1(i);

                UpdatePlayerMoneyText(player.Id, player.Resources);
            }
            
            foreach (var i in addMoneyFilter)
            {
                ref var player = ref addMoneyFilter.Get1(i);

                UpdatePlayerMoneyText(player.Id, player.Resources);
            }
        }

        void UpdatePlayerMoneyText(int playerId, float money)
        {
            foreach (var w in filter)
            {
                ref var moneyText = ref filter.Get1(w);
                    
                if (playerId == moneyText.PlayerId)
                    SetText(ref moneyText, money);
            }
        }

        void SetText(ref MoneyTextComponent money, float value) => money.Text.text = "Money: $" + Mathf.RoundToInt(value);
    }
}