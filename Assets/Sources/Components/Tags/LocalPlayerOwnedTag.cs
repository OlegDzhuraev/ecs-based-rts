using Leopotam.Ecs;

namespace InsaneOne.EcsRts 
{
    /// <summary> Marks all unit, owned by player, not AI. </summary>
    struct LocalPlayerOwnedTag : IEcsIgnoreInFilter { }
}