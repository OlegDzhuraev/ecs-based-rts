using UnityEngine;

namespace Sources.Storing
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
    }
}