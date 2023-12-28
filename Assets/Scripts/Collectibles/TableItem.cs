using System.Collections;
using System.Collections.Generic;
using Collectibles;
using UnityEngine;

[CreateAssetMenu(fileName = "New Table Item", menuName = "Inventory Table Item", order = 1)]
public class TableItem : InventoryCollectibleItem
{
    [SerializeField] private bool hasLegs; // Temp placeholder field unique to class
    
    public override InventoryCollectibleItem GetItem() { return this; }
    public override TableItem GetTable() { return this; }
    public override CubeItem GetCube() { return null; }
    public override GiftItem GetGift() { return null; }
    public override SphereItem GetSphere() { return null; }
}