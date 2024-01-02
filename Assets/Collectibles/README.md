# Collectible Items

There are 2 parts to each item:
- The `InventoryCollectibleItem` scriptable object, which contains the actual item information:
  - Item ID (for saving/loading); `string`, e.g. `blueCloud1`
  - Thumbnail to display in the UI; `Sprite`
  - Name; `string`, e.g. `Blue Cloud 1`
  - Description; `string`, e.g. `A variation of a Blue Cloud`

- The `PlacedCollectibleItem` component, which goes on a game object in the scene. 
This contains a reference to the `InventoryCollectibleItem` that the object should give the player when they collect it.

Any new Inventory Collectible Items should be added into the List on the `Collectible Item Registry` object in the 
`Assets/Collectibles` directory, in order to have references to every available item when loading data.
This can be done automatically by going to `LTG8` in the toolbar and selecting `Auto Register Items`. 
It will clear the items currently in the registry, and add all items it can find in the `Assets/Collectibles` directory.
It will also show a warning if there are item ids that are used more than once.

When Placed Items are collected, they are removed from the scene and the associated Inventory Items are added to 
the list in the `PlayerCollectibleItemController` component on the player. 

In the future, when saving and loading is implemented, items that are already collected should be 
disabled/destroyed when the scene loads.

Inventory collectible items are currently stored in `Assets/Collectibles`.