using UnityEngine;

namespace InsaneOne.EcsRts.Storing
{
    [CreateAssetMenu]
    public class UnitData : ScriptableObject
    {
        public GameObject Prefab;
        
        [Header("Move parameters")]
        public MoveData Move;

        [Header("Defense parameters")] 
        public DefenseData Defense;
        
        [Header("Attack parameters")] 
        public AttackData Attack;

        [Header("Production parameters")]
        public ProductionData Production;
        
        [Header("Effects parameters")]
        public EffectsData Effects;      
        
        [Header("Harvest parameters")]
        public HarvestData Harvest;
    }

    [System.Serializable]
    public struct ProductionData
    {
        public bool CanProduceUnits;
        public UnitData[] Units;
        public float SelfProduceTime;
        public int Price;
    }

    [System.Serializable]
    public struct MoveData
    {
        public bool CanMove;
        public float MoveSpeed;
        public float RotationSpeed;
        public float StopDistance;
    }
    
    [System.Serializable]
    public struct DefenseData
    {
        public float MaxHealth;
    }
    
    [System.Serializable]
    public struct AttackData
    {
        public bool CanAttack;
        public float Damage;
        public float ReloadTime;
        public bool HaveTurret;
    }
    
    [System.Serializable]
    public struct HarvestData
    {
        public bool CanHarvest;
        public float HarvestSpeed;
        public int MaxResourcesAmount;
    }
    
    [System.Serializable]
    public struct EffectsData
    {
        public GameObject ShootEffect;
    }
}