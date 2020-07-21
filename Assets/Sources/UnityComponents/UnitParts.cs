using UnityEngine;

namespace InsaneOne.UnityComponents
{
    /// <summary> This class used as a bridge to the ECS. Data being gathered from this one and forwarded to the ECS components. </summary>
    public class UnitParts : MonoBehaviour
    {
        public Renderer[] ColoredRenderers;
        public Transform Turret;
        public Transform ShootPoint;
    }
}