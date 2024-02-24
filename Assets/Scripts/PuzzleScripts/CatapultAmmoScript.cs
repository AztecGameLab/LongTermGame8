using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using Ltg8.Player;
using UnityEngine;

public class CatapultAmmoScript : MonoBehaviour
{
    public Transform loadPosition;
    public Transform unloadPosition;

    private bool _loaded;

    private GameObject _loadedItem;

    private void OnGUI()
    {
        string output;
        if (_loadedItem != null)
        {
            output = _loadedItem.name;
        }
        else
        {
            output = "null";
        }

        GUILayout.Label(output);
    }

    public void ObjectLoaded(Rigidbody enteredRb)
    {
        if (_loaded && _loadedItem != null) return;
        
        _loaded = true;
        _loadedItem = enteredRb.gameObject;
    }

    public void ObjectUnloaded(Rigidbody exitedRb)
    {
        if (_loadedItem == null || exitedRb.gameObject != _loadedItem) return;
        
        _loaded = false;
        _loadedItem = null;
    }

    public void PlacePlayer(GameObject player)
    {
        if (_loaded) return;

        player.transform.position = loadPosition.position;
    }

    public async void PlaceItem(ItemData item)
    {
        if (_loaded) return;

        await InventoryUtil.CreateItemInOverworld(item, loadPosition.position);
    }

    public void Unload()
    {
        if (_loadedItem == null) return;

        if (_loadedItem.TryGetComponent(out PlayerController player))
        {
            player.transform.position = unloadPosition.position;
        }
        else
        {
            Destroy(_loadedItem);
        }

        _loaded = false;
        _loadedItem = null;
    }

    // Also should be called for firing the catapult
    public void Reset()
    {
        _loadedItem = null;
        _loaded = false;
    }
}