﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceMystery;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ltg8.Misc
{
    public static class UniTaskTween
    {
        public static EasingFunction DefaultEasing { get; set; } = Easing.SmoothStart2;

        public static async UniTask TweenLocalScale(this Transform transform, Vector3 target, TweenSettings settings, CancellationToken token = default)
        {
            await TweenLocalScale(transform, target, settings.duration, settings.easingFunction.Resolve(), token);
        }
        
        public static async UniTask TweenLocalScale(this Transform transform, Vector3 target, 
            float duration = 1, EasingFunction easingFunction = null, CancellationToken token = default)
        {
            easingFunction ??= DefaultEasing;
            float elapsed = 0;
            Vector3 start = transform.localScale;
            
            while (elapsed < duration && !token.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                float t = easingFunction(Mathf.Clamp01(elapsed / duration));
                transform.localScale = Vector3.Lerp(start, target, t);
                await UniTask.Yield();
            }
        }
    }
    
    [Serializable]
    public class TweenSettings
    {
        public float duration;
        public EasingFunctionReference easingFunction;
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TweenSettings))]
    public class TweenSettingsPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);
            
            Rect durationRect = position;
            durationRect.width *= 0.25f;
            
            Rect easingRect = position;
            easingRect.width *= 0.75f;
            easingRect.x += durationRect.width;
            
            property.Next(true);
            EditorGUI.PropertyField(durationRect, property, GUIContent.none);
            
            property.Next(true);
            EditorGUI.PropertyField(easingRect, property, GUIContent.none);
        }
    }
#endif
}
