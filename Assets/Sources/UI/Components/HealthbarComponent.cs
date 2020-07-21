using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.EcsRts.UI
{
    struct HealthbarComponent
    {
        public GameObject SelfObject;
        public UnitComponent UnitComponent;
        public Image FillImage;
    }
}