using Sources.Storing;
using UnityEngine;

namespace Sources.Components
{
    struct AttackComponent
    {
        public AttackData Data;

        public Transform ShootPoint;
            
        public bool IsReloading;
        public float ReloadTimeLeft;
    }
}