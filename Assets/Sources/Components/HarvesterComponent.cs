using InsaneOne.EcsRts.Storing;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts 
{
    struct HarvesterComponent
    {
	    public HarvestData Data;
	    
	    public float ResourcesAmount;

	    public Vector3 GiveResourcesPoint;
	    public EcsEntity SelectedFieldEntity;
	    public Vector3 SelectedFieldPos;
    }
}