using System.Collections.Generic;
using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.EcsRts.UI 
{
    sealed class BuyButtonsSystem : IEcsInitSystem, IEcsRunSystem 
    {
        readonly GameStartData gameStartData = null;
        
        readonly EcsFilter<ShowBuyButtonsEvent> showButtonsFilter = null;
        readonly EcsFilter<HideBuyButtonsEvent> hideButtonsFilter = null;
        
        readonly List<GameObject> drawnButtons = new List<GameObject>();
        
        RectTransform buttonsPanel;

        void IEcsInitSystem.Init()
        {
            var buttonsPanelObj = GameObject.FindWithTag("UI/BuyButtonsPanel");
            buttonsPanel = buttonsPanelObj.GetComponent<RectTransform>();
        }
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in showButtonsFilter) // todo check is foreach really needed for 1 event?
            {
                ClearDrawnButtons();
                
                ref var showEvent = ref showButtonsFilter.Get1(i);

                foreach (var unitData in showEvent.Production.Data.Units)
                    CreateUnitBuyButton(showEvent.ProductionEntity, unitData);
            }

            foreach (var i in hideButtonsFilter)
            {
                ClearDrawnButtons();
                break; // doing it once
            }
        }

        void CreateUnitBuyButton(EcsEntity productionEntity, UnitData unitData)
        {
            var buttonObj = GameObject.Instantiate(gameStartData.BuyButtonTemplate, buttonsPanel);
            var button = buttonObj.GetComponent<Button>();
            
            button.onClick.AddListener(delegate { BuyButtonClick(productionEntity, unitData); });
            button.transform.Find("Text").GetComponent<Text>().text = unitData.name;
            
            drawnButtons.Add(buttonObj);
        }

        void BuyButtonClick(EcsEntity productionEntity, UnitData unitData)
        {
            var ownerPlayer = productionEntity.Get<UnitComponent>().OwnerPlayer; // todo too much gets, make it better
            
            if (ownerPlayer.Get<PlayerComponent>().Resources < unitData.Production.Price)
                return;
            
            ownerPlayer.Get<SpendPlayerResourcesEvent>().Value = unitData.Production.Price;
            productionEntity.Get<RequestBuyUnitEvent>().UnitData = unitData;
        }

        void ClearDrawnButtons()
        {
            for (var i = 0; i < drawnButtons.Count; i++)
                GameObject.Destroy(drawnButtons[i]);
            
            drawnButtons.Clear();
        }
    }
}