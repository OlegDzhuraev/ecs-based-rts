using UnityEngine;

namespace InsaneOne.EcsRts.UI
{
    struct FloatingComponent
    {
        public RectTransform SelfTransform;
        public Transform FollowTransform;
        public float Height;
    }
}