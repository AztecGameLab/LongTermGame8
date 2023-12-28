using System.Collections;
using System.Collections.Generic;
using Collectibles;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sphere Item", menuName = "Inventory Sphere Item", order = 1)]
public class SphereItem : InventoryCollectibleItem
{
    public bool isBox;
    
    public override InventoryCollectibleItem GetItem() { return this; }
    public override SphereItem GetSphere() { return this; }
    public override CubeItem GetCube() { return null; }
    public override GiftItem GetGift() { return null; }
    public override TableItem GetTable() { return null; }
}