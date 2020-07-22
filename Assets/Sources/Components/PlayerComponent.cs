using UnityEngine;

namespace InsaneOne.EcsRts 
{
    struct PlayerComponent
    {
        public const int LocalPlayerId = 0;
        
        public int Id;
        public float Resources;
        public Color Color;
    }
}