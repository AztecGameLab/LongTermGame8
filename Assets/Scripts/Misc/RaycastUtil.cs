using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Misc
{
    public static class RaycastUtil
    {
        public static T FindNearestWithComponent<T>(int hits, [NotNull] RaycastHit[] hitBuffer, Predicate<RaycastHit> isValidPredicate = null)
        {
            T result;
            
            RaycastHit hit = FindNearest(hits, hitBuffer, hit => 
                (isValidPredicate == null || isValidPredicate(hit)) && /* always ensure the custom-requirements hold*/
                (
                    hit.rigidbody != null && hit.rigidbody.TryGetComponent<T>(out _) ||  /* first, check if rigidbody has component */
                    hit.collider.TryGetComponent<T>(out _)) /* as a fallback, check if the collider has component */
                );

            // extract the component from our resulting hit, and return it
            if (hit.rigidbody != null && hit.rigidbody.TryGetComponent(out T comp)) result = comp;
            else hit.collider.TryGetComponent(out result);
            return result;
        }
        
        public static RaycastHit FindNearest(int hits, [NotNull] RaycastHit[] hitBuffer, Predicate<RaycastHit> isValidPredicate = null)
        {
            Assert.IsTrue(hits > 0);
            float nearestDist = float.PositiveInfinity;
            RaycastHit nearest = hitBuffer[0];

            for (int i = 0; i < hits; ++i)
            {
                if (hitBuffer[i].distance < nearestDist)
                {
                    if (isValidPredicate == null || isValidPredicate(hitBuffer[i]))
                    {
                        nearest = hitBuffer[i];
                        nearestDist = hitBuffer[i].distance;
                    }
                }
            }

            return nearest;
        }
    }
}
