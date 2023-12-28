using System.Collections;
using System.Collections.Generic;
using Collectibles;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cube Item", menuName = "Inventory Cube Item", order = 1)]
public class CubeItem : InventoryCollectibleItem
{
    [SerializeField] private bool isBox; // Temp placeholder field unique to class
    
    public override InventoryCollectibleItem GetItem() { return this; }
    public override CubeItem GetCube() { return this; }
    public override GiftItem GetGift() { return null; }
    public override SphereItem GetSphere() { return null; }
    public override TableItem GetTable() { return null; }
    
}