using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectibles
{
    public abstract class PlayerInteractable : MonoBehaviour
    {
        /// <summary>
        /// Method to handle interaction from the player. <br/>
        /// 
        /// If other components are needed, i.e. the player's inventory,
        /// those should be added with public accessors/getters in <see cref="T:Collectibles.PlayerInteractController"/>.
        /// </summary>
        /// <param name="playerInteractController"></param>
        public abstract void Interact(PlayerInteractController playerInteractController);
        
        public bool interactionEnabled = true;
        public Transform labelPosition;
        public GameObject labelPrefab;

        private bool _highlighted;
        private GameObject _label;
 
        private void Start()
        {
            _label = Instantiate(labelPrefab, labelPosition);
        }

        public void Highlight(bool value = true)
        {
            _highlighted = value;

            if (_label.activeSelf == value) 
                return;
            
            _label.SetActive(value);
        }

        private void LateUpdate()
        {
            if (_highlighted == false)
            {
                Highlight(false);
            }
            
            _highlighted = false;
        }
        
    }
}