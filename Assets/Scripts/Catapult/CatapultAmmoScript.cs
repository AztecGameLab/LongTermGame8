using Ltg8.Inventory;
using UnityEditor;
using UnityEngine;

namespace Catapult
{
    public class CatapultAmmoScript : MonoBehaviour
    {
        
        // Proximity Interactable for launching whilst in the basket
        [SerializeField] private GameObject launchBasketProx;
        
        // Position where Catapult Projectiles are loaded into
        [SerializeField] private Transform loadPosition;
        
        // Uses the script inside the catapult
        [SerializeField] private CatapultLaunchScript launchScript;
        
        // The item loaded into the catapult
        private GameObject _loadedItem;
        
        // Not really sure what this does tbh
        private void OnGUI()
        {
            var output =
                // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                _loadedItem != null ? _loadedItem.name : "null";

            GUILayout.Label(output);
        }

        // This function sets an object as "Loaded" when it enters the Catapult Basket
        public void ObjectLoaded(GameObject enteredObject)
        {
            if (_loadedItem != null) return; // If an item is already loaded, return
            _loadedItem = enteredObject; // Set the Catapult's Loaded Item as the entered object
            launchScript.SetProjectile(enteredObject); // Set the Catapult's projectile to the loaded object
            var enteredObjectRb = enteredObject.GetComponent<Rigidbody>();
            // Make sure the item has no physics that can inhibit the catapult's animation
            enteredObjectRb.useGravity = false;
            enteredObjectRb.angularDrag = 0;
            enteredObject.AddComponent<TouchGrass>(); // Adds the TouchGrass script to the projectile
            enteredObject.GetComponent<BoxCollider>().size = enteredObject.transform.localScale;
            CreateSphereCollider(enteredObject); 
        }

        // This function removes an object from being loaded when it exits the Catapult Basket
        public void ObjectUnloaded(GameObject exitedObject)
        {
            if (_loadedItem == null) return; // If the catapult isn't loaded, return
            // If the item being removed from the catapult was not launched from the catapult
            if (!launchScript.GetLaunchedObject() == _loadedItem) 
            {
                Invoke(nameof(RemoveItem),0.1f); 
            }
            // Set the catapult's loaded item to nothing
            _loadedItem = null;
        }

        // Places the player into the catapult 
        public void PlacePlayer(GameObject player)
        {
            if (_loadedItem) { // If the catapult is already loaded
                // The item needs to exit the trigger to execute ObjectUnloaded
                _loadedItem.transform.position += new Vector3(0,-3,0); 
                // Deactivates the item so the player can't see it
                _loadedItem.SetActive(false);
                return;} // Return, the player will have to call the function again
            if (!player.TryGetComponent(out CharacterController _)) return; // If object is not a player
            // Temporarily disable the player's CharacterController to prevent User Error
            player.GetComponent<CharacterController>().enabled = false; 
            player.transform.position = loadPosition.position; // Place the player in the catapult basket
            // Once the player is in the basket, allow them to launch from the basket
            launchBasketProx.SetActive(true); 
            /* Calls the function directly because CharacterController is disabled, and therefore the
             player cannot activate the trigger the usual way*/
            ObjectLoaded(player); 
            Invoke(nameof(DelayCharacterController),0.5f);
        }

        // Removes the player from the catapult
        public void UnplacePlayer(GameObject player)
        {
            // If object is not a player, return
            if (!player.TryGetComponent(out CharacterController playerController)) return; 
            /* If CharacterController is disabled (meaning the player was removed from the catapult as a
             projectile, not a player), do not use this function*/
            if (!playerController.enabled) return;
            launchBasketProx.SetActive(false); // Deactivate the Basket's Proximity Interactable
            launchScript.SetProjectile(null); // Set the Catapult's Projectile to nothing
        }

        // Create and Load an item into the catapult
        public async void PlaceItem(ItemData item)
        {
            if (_loadedItem) return; // If the catapult is already loaded, return

            // Create the object chosen by the player in the Overworld and place it into the basket
            await InventoryUtil.CreateItemInOverworld(item, loadPosition.position);
        }

        // Remove an item inside the catapult (Only called when catapult did not launch)
        private void RemoveItem()
        {
            // If there is no loaded item or the loaded item is a player, return 
            if (!_loadedItem || _loadedItem.TryGetComponent(out CharacterController _)) return;
            Destroy(_loadedItem); // Destroy the item
            launchScript.SetProjectile(null); // Set the catapult's projectile to nothing
        }
    
        // Enables the CharacterController, typically via the Invoke command (delays execution)
        private void DelayCharacterController()
        {
            _loadedItem.GetComponent<CharacterController>().enabled = true;
        }

        // Creates a Sphere Collider Trigger in the projectile, to detect when it hits the ground
        private static void CreateSphereCollider(GameObject gameObject)
        {
            var enteredObjectSc = gameObject.AddComponent<SphereCollider>();
            // Make the collider a trigger, but don't enable the Collider yet
            enteredObjectSc.isTrigger = true;
            enteredObjectSc.enabled = false;
            // Set the radius of the Sphere Collider to be 0.15 above the object's length
            enteredObjectSc.radius = gameObject.transform.localScale.x + 0.15f;
        }
    }
}