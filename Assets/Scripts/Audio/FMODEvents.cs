using System;
using UnityEngine;
using UnityEngine.Serialization;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
   [field: Header("Walking Action Audio")] 
   [field: SerializeField] public EventReference GroundWalkingSfx { get; private set; }
   [field: Header("Jump Action Audio")] 
   [field: SerializeField] public EventReference GroundJumpSfx { get; private set; }
   
   public static FMODEvents Instance { get; private set; }
   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("More than one FMOD Events instance in scene");
      }

      Instance = this;
   }
   
}
