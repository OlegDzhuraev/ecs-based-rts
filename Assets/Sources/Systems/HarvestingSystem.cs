using System.Collections.Generic;
using Leopotam.Ecs;
using Sources;
using UnityEngine;

namespace InsaneOne.EcsRts 
{
    // looks like this system version is not good and requires refactor
    sealed class HarvestingSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld world;
        
        readonly EcsFilter<HarvesterComponent, UnitComponent>.Exclude<HarvestingTag, HarvesterGiveResourcesTag> idleHarvestersFilter = null;
        readonly EcsFilter<HarvesterComponent, UnitComponent, HarvestingTag> harvestingFilter = null;
        readonly EcsFilter<HarvesterComponent, UnitComponent, HarvesterGiveResourcesTag> giveResourcesFilter = null;
       
        readonly EcsFilter<ResourceFieldComponent, SpendFieldResourcesEvent>.Exclude<ResourcesFieldEmptyTag> resourceFieldsFilter = null;

        readonly List<Transform> actualFieldsTransforms = new List<Transform>();
        readonly List<EcsEntity> actualFields = new List<EcsEntity>();
        
        public void Init()
        {
            var fields = GameObject.FindGameObjectsWithTag("ResourceField");

            foreach (var fieldObject in fields)
            {
                var entity = world.NewEntity();
                ref var field = ref entity.Get<ResourceFieldComponent>();

                field.Transform = fieldObject.transform;
                field.ResourcesLeft = 10000; // todo change this hardcode
                
                actualFieldsTransforms.Add(field.Transform);
                actualFields.Add(entity);
            }
        }
        
        void IEcsRunSystem.Run ()
        {
            ProcessFieldSelect();
            ProcessHarvest();
            ProcessGiveResources();

            CheckEmptyFields();
        }

        void CheckEmptyFields()
        {
            foreach (var i in resourceFieldsFilter)
            {
                ref var field = ref resourceFieldsFilter.Get1(i);
                ref var spendEvent = ref resourceFieldsFilter.Get2(i);

                field.ResourcesLeft -= spendEvent.Value;
                
                if (field.ResourcesLeft <= 0)
                {
                    resourceFieldsFilter.GetEntity(i).Get<ResourcesFieldEmptyTag>();
                    
                    actualFieldsTransforms.Remove(field.Transform);
                    actualFields.Remove(resourceFieldsFilter.GetEntity(i));
                    
                    resourceFieldsFilter.GetEntity(i).Destroy(); // to exclude this from search, temporary, in future use ResourcesFieldEmptyTag only
                }
            }
        }

        void ProcessFieldSelect()
        {
            foreach (var i in idleHarvestersFilter)
            {
                var harvEnt = idleHarvestersFilter.GetEntity(i);
                ref var harvester = ref idleHarvestersFilter.Get1(i);
                ref var unit = ref idleHarvestersFilter.Get2(i);
                
                var fieldTransform = unit.Transform.position.GetNearestObjectOfType(actualFieldsTransforms);
                var id = actualFieldsTransforms.IndexOf(fieldTransform);
                
                harvester.SelectedFieldEntity = actualFields[id];
                harvester.SelectedFieldPos = fieldTransform.position;
                harvEnt.Get<MoveOrderEvent>().DestinationPosition = fieldTransform.position;
                harvEnt.Get<HarvestingTag>();
            }
        }

        void ProcessHarvest()
        {
            foreach (var i in harvestingFilter)
            {
                ref var harvester = ref harvestingFilter.Get1(i);
                ref var unit = ref harvestingFilter.Get2(i);
                var entity = harvestingFilter.GetEntity(i);
               
                if (Vector3.Distance(unit.Position, harvester.SelectedFieldPos) > 1f)
                    continue;

                if (harvester.ResourcesAmount < harvester.Data.MaxResourcesAmount && !harvester.SelectedFieldEntity.Has<ResourcesFieldEmptyTag>())
                {
                    var harvestPerFrame = Time.deltaTime * harvester.Data.HarvestSpeed;
                    harvester.ResourcesAmount += harvestPerFrame;
                }
                else
                {
                    harvester.SelectedFieldEntity.Get<SpendFieldResourcesEvent>().Value = harvester.ResourcesAmount;
                    
                    entity.Del<HarvestingTag>();
                    entity.Get<HarvesterGiveResourcesTag>();
                    entity.Get<MoveOrderEvent>().DestinationPosition = harvester.GiveResourcesPoint;
                }
            }
        }

        void ProcessGiveResources()
        {
            foreach (var i in giveResourcesFilter)
            {
                ref var harvester = ref giveResourcesFilter.Get1(i);
                ref var unit = ref giveResourcesFilter.Get2(i);
                var entity = harvestingFilter.GetEntity(i);
                
                if (Vector3.Distance(unit.Position, harvester.GiveResourcesPoint) > 1f)
                    continue;
                
                // todo check why it OwnerPlayer cn be null, it should exist all time.
                if (unit.OwnerPlayer.IsAlive())
                    unit.OwnerPlayer.Get<AddPlayerResourcesEvent>().Value = (int) harvester.ResourcesAmount;

                harvester.ResourcesAmount = 0;
                entity.Del<HarvesterGiveResourcesTag>();
            }
        }
    }
}