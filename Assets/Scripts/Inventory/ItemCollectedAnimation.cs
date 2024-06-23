using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Inventory
{
    
    /*
     * Description
     * This class is responsible for the functionality of picking up / collecting items. This includes
     * saving the item into the player's inventory data, and (mostly) dealing with the animation that occurs.
     * NOTE: Research Timeline
     */
    
    public class ItemCollectedAnimation : MonoBehaviour 
    {
        // Controller for the animation that plays when you pick up an item NOTE: (Not to familiar with this)
        [SerializeField] private PlayableDirector timeline; 
        [SerializeField] private Transform uiViewParent; // The new parent of the instantiated 2D item
        [SerializeField] private TMP_Text nameText; // The name of the item

        private GameObject _prevInstance; // The previous instance of the object (2D sticker)
        
        public async UniTask Play(InventoryItemData item)
        {
            // If there is a previous instance, destroy it. This would typically happen if the animation has already
            // played once before.
            if (_prevInstance != null) Destroy(_prevInstance.gameObject); 
            nameText.SetText(item.Data.itemName); // Set the the name text to the item's name
            // Instantiate (create) the 2D sticker, and assign it to the previous instance
            _prevInstance = Instantiate(item.Data.uiView, uiViewParent); 
            // Hides the canvas group in the object that holds this script
            /* NOTE: CanvasGroup allows you to control multiple UI elements with just the one property. Modify
             * NOTE: both the gameObject the component is in and its children */
            GetComponent<CanvasGroup>().alpha = 1;
            // Sets the scale of this script's object to (1,1,1)
            transform.localScale = Vector3.one;
            // Plays the playables on the timeline NOTE: (not too familiar with this)
            timeline.Play();
            // Adds the item to the player's saved inventory
            Ltg8.Ltg8.Save.Inventory.Add(item);
            // Waits until the timeline playables (item collected animation fade in) are done playing
            // NOTE: (Not too familiar with this)
            await UniTask.WaitUntil(() => timeline.state != PlayState.Playing);
            // Waits until a key is pressed by the player
            await UniTask.WaitUntil(() => Input.anyKeyDown);
            // Resumes the timeline playables (item collected animation fade out) 
            timeline.Resume();
            // Waits until these playables are done playing
            await UniTask.WaitUntil(() => timeline.state != PlayState.Playing);
        }
    }
}
