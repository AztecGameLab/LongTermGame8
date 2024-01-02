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
        
        private List<Material> _materials;
        private bool _highlighted;
        
        protected virtual void Start()
        {
            _materials = new List<Material>(GetComponent<Renderer>().materials);
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