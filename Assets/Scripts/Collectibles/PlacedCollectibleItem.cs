using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public class PlacedCollectibleItem : MonoBehaviour, IPlayerInteractable
    {
        public InventoryCollectibleItem collectibleItem;

        private List<Material> _materials;
        private bool _highlighted;

        private void Start()
        {
            _materials = new List<Material>(GetComponent<Renderer>().materials);
        }

        public void Interact(PlayerInteractController playerInteractController)
        {
            playerInteractController.Inventory.collectedItems.Add(collectibleItem);
            gameObject.SetActive(false);
        }
        
        private static readonly string EMISSION = "_EMISSION";
        private static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");
        public void Highlight(Color color, bool value = true)
        {
            _highlighted = value;
            foreach (Material material in _materials)
            {
                material.SetColor(EMISSION_COLOR, color);
                
                if (material.IsKeywordEnabled(EMISSION) == value)
                    continue;

                if (value)
                {
                    material.EnableKeyword(EMISSION);
                }
                else
                {
                    material.DisableKeyword(EMISSION);
                }
                
            }
        }

        public void Highlight(bool value = true)
        {
            Highlight(Color.white, value);
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