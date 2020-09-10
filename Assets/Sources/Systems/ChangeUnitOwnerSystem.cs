using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    public class ChangeUnitOwnerSystem : IEcsRunSystem
    {
	    readonly GameStartData gameStartData;
	    
	    readonly EcsFilter<UnitComponent, ColoredRenderersComponent, ChangeUnitOwnerEvent> unitFilter = null;
	    
	    static readonly int colorId = Shader.PropertyToID("_Color");

	    public void Run()
        {
	        foreach (var i in unitFilter)
	        {
		        var entity = unitFilter.GetEntity(i);
		        
		        ref var unit = ref unitFilter.Get1(i);
		        ref var coloredRenderers = ref unitFilter.Get2(i);
		        ref var changeUnitOwnerEvent = ref unitFilter.Get3(i);

		        unit.OwnerPlayer = changeUnitOwnerEvent.NewOwnerPlayerEntity;
		        unit.OwnerPlayerId = changeUnitOwnerEvent.NewOwnerPlayerId;

		        if (unit.OwnerPlayerId == PlayerComponent.LocalPlayerId)
		        {
			        entity.Get<LocalPlayerOwnedTag>();
			        entity.Del<UnitAiComponent>();
		        }
		        else
		        {
			        entity.Del<LocalPlayerOwnedTag>();
			        entity.Get<UnitAiComponent>();
		        }

		        foreach (var renderer in coloredRenderers.Renderers)
			        renderer.material.SetColor(colorId, gameStartData.PlayerColors[unit.OwnerPlayerId]);
	        }
        }
    }
}