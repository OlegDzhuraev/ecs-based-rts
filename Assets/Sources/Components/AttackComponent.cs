using InsaneOne.EcsRts.Storing;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    struct AttackComponent
    {
        public AttackData Data;

        public Transform ShootPoint;
            
        public bool IsReloading;
        public float ReloadTimeLeft;
    }
}