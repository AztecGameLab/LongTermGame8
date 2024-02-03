using System;
using UnityEngine;

namespace SpaceMystery
{
    public delegate float EasingFunction(float t);

    [Serializable]
    public class EasingFunctionReference /* todo: nice property drawer? would like a single-file solution, if possible? */
    {
        [SerializeField]
        private Easing.Type type = Easing.Type.SmoothStart2;

        public EasingFunction Resolve()
        {
            return Easing.LookupFunction(type);
        }
    }
    
    public static class Easing
    {
        public enum Type
        {
            SmoothStop2, SmoothStop3, SmoothStop4,
            SmoothStart2, SmoothStart3, SmoothStart4,
        }

        public static EasingFunction LookupFunction(Type type)
        {
            switch (type)
            {
                case Type.SmoothStop2: return SmoothStop2;
                case Type.SmoothStop3: return SmoothStop3;
                case Type.SmoothStop4: return SmoothStop4;
                case Type.SmoothStart2: return SmoothStart2;
                case Type.SmoothStart3: return SmoothStart3;
                case Type.SmoothStart4: return SmoothStart4;
                default: throw new Exception("Invalid easing type!");
            }
        }
        
        public static float SmoothStop2(float t) => 1 - (1 - t) * (1 - t);
        public static float SmoothStop3(float t) => 1 - (1 - t) * (1 - t) * (1 - t);
        public static float SmoothStop4(float t) => 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t);

        public static float SmoothStart2(float t) => t * t;
        public static float SmoothStart3(float t) => t * t * t;
        public static float SmoothStart4(float t) => t * t * t * t;
    }
}
