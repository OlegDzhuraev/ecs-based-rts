using UnityEngine;

namespace InsaneOne.EcsRts 
{
    struct PlayerComponent
    {
        public const int LocalPlayerId = 0;
        
        public int Id;
        public int Money;
        public Color Color;
    }
}