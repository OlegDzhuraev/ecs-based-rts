using Leopotam.Ecs;
using Sources.Components;
using Sources.Components.Events;
using Sources.Storing;
using UnityEngine;

namespace Sources.Systems
{
    public class ChangeUnitOwnerSystem : IEcsRunSystem
    {
	    readonly GameStartData gameStartData;
	    
	    readonly EcsFilter<UnitComponent, ColoredRenderers, ChangeUnitOwnerEvent> unitFilter = null;
	    
	    static readonly int colorId = Shader.PropertyToID("_Color");

	    public void Run()
        {
	        foreach (var i in unitFilter)
	        {
		        ref var unit = ref unitFilter.Get1(i);
		        ref var coloredRenderers = ref unitFilter.Get2(i);
		        ref var changeUnitOwnerEvent = ref unitFilter.Get3(i);

		        unit.OwnerPlayerId = changeUnitOwnerEvent.NewOwnerPlayerId;
		        
		        foreach (var renderer in coloredRenderers.Renderers)
			        renderer.material.SetColor(colorId, gameStartData.PlayerColors[unit.OwnerPlayerId]);
	        }
        }
    }
}