using System.Collections.Generic;
using FMOD.Studio;
using JetBrains.Annotations;
using poetools.Console.Commands;
using UnityEngine;

namespace Ltg8
{
    public class PersistentAudio : MonoBehaviour, IConsoleDebugInfo
    {
        /// <summary>
        /// A collection of FMOD instances that should persistent between scenes.
        /// </summary>
        [PublicAPI]
        public readonly Dictionary<string, EventInstance> Instances = new Dictionary<string, EventInstance>();

        public string DebugName => name;
        private readonly List<string> _debugRemovalQueue = new List<string>();
    
        public void DrawDebugInfo()
        {
            GUILayout.Label($"Persistent Audio: {Instances.Count}");
            _debugRemovalQueue.Clear();

            foreach (string id in Instances.Keys)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(id);
                if (GUILayout.Button("x"))
                {
                    Instances[id].stop(STOP_MODE.ALLOWFADEOUT);
                    Instances[id].release();
                    _debugRemovalQueue.Add(id);
                }
                GUILayout.EndHorizontal();
            }

            foreach (string idToRemove in _debugRemovalQueue)
                Instances.Remove(idToRemove);
        }
    }
}
