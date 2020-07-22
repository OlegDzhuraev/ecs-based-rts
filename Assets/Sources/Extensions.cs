using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    public static class Extensions
    {
        public static T GetNearestObjectOfType<T>(this Vector3 position, List<T> objectsToSelectFrom) where T : Component
        {
            T nearestObject = null;
            float currentNearestDistance = float.MaxValue - 1;

            for (int i = 0; i < objectsToSelectFrom.Count; i++)
            {
                float curDistance = (position - objectsToSelectFrom[i].transform.position).sqrMagnitude;

                if (curDistance < currentNearestDistance)
                {
                    nearestObject = objectsToSelectFrom[i];
                    currentNearestDistance = curDistance;
                }
            }

            return nearestObject;
        }
    }
}