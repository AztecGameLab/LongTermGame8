using System.Collections;
using System.Collections.Generic;
using Catapult;
using Ltg8.Inventory;
using Ltg8.Player;
using UnityEngine;

public class CatapultAmmoScript : MonoBehaviour
{
    public Transform loadPosition;
    public Transform unloadPosition;

    [SerializeField] private bool _loaded;

    public GameObject _loadedItem;

    [SerializeField] private GameObject launchBasketProx;
    [SerializeField] private GameObject catapult;
    [SerializeField] private GameObject catapult_basket;

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

    public void ObjectLoaded(GameObject enteredObject)
    {
        if (_loaded && _loadedItem != null) return;
        enteredObject.GetComponent<Rigidbody>().useGravity = false;
        enteredObject.GetComponent<Rigidbody>().angularDrag = 0;
        enteredObject.AddComponent<SphereCollider>();
        enteredObject.AddComponent<TouchGrass>();
        enteredObject.GetComponent<SphereCollider>().isTrigger = true;
        enteredObject.GetComponent<SphereCollider>().enabled = false;
        enteredObject.GetComponent<SphereCollider>().radius = 0.65f;
        catapult.GetComponent<CatapultLaunchScript>().SetProjectile(enteredObject);
        _loaded = true;
        _loadedItem = enteredObject;
    }

    public void ObjectUnloaded(GameObject exitedObject)
    {
        if (_loadedItem == null || exitedObject != _loadedItem) return;
        if (!catapult.GetComponent<CatapultLaunchScript>().launched_object == _loadedItem)
        {
            StartCoroutine(RemoveItem(_loadedItem, 0.1f));
        }
        _loaded = false;
        _loadedItem = null;
    }

    public void PlacePlayer(GameObject player)
    {
        if (_loaded) {Unload(); return;}

        if (player.TryGetComponent(out CharacterController playerController))
        {
            //playerController.Move(loadPosition.position - player.transform.position);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = loadPosition.position;
            StartCoroutine(DelayProx(0.5f,player));
            _loaded = true;
            _loadedItem = player;
            catapult.GetComponent<CatapultLaunchScript>().SetProjectile(player);
        }
    }

    public void UnplacePlayer(GameObject player)
    {
        if (player.TryGetComponent(out CharacterController playerController))
        {
            if (playerController.enabled)
            {
                launchBasketProx.SetActive(false);
                _loaded = false;
                _loadedItem = null;
                catapult.GetComponent<CatapultLaunchScript>().SetProjectile(null);
                Debug.Log("Unplace Player");
                catapult_basket.GetComponent<FixedJoint>().connectedBody = null;
            }
        }
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
            _loadedItem.transform.position += new Vector3(0,-3,0);
            _loadedItem.SetActive(false);
        }
    }

    private IEnumerator<WaitForSeconds> RemoveItem(GameObject item, float time)
    {
        yield return new WaitForSeconds(time);
        if (item && !(item.TryGetComponent(out CharacterController characterController)))
        {
            Destroy(item);
            Debug.Log("After Destroy");
            catapult_basket.GetComponent<FixedJoint>().connectedBody = null;
            catapult.GetComponent<CatapultLaunchScript>().SetProjectile(null);
        }
    }

    // Also should be called for firing the catapult
    public void Reset()
    {
        /*_loadedItem = null;
        _loaded = false;*/
    }
    
    private IEnumerator<WaitForSeconds> DelayProx(float time, GameObject player)
    {
        yield return new WaitForSeconds(time);
        launchBasketProx.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<CharacterController>().enabled = true;
    }
}