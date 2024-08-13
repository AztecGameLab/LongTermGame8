using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: (This class appears to be unfinished)
    /// </summary>
    /*
     * Description
     * From what I can gather, this class creates two arrays.
     * One is an int array, which holds values that can be accessed using Var enums as the indices
     * The other is a boolean array, which I assume is to determine whether a value in the int array is set or not
     * NOTE: Complete
     */
    public class SaveData
    {
        // Variables meant to store critical player save data (Like current position, scene, & Inventory)
        public Vector3 PlayerPos;
        public int PlayerSceneId;
        public List<InventoryItemData> Inventory = new List<InventoryItemData>();

        // Creates a boolean array, the size of which is equal to the amount of Flag Enum values
        public bool[] Flags = new bool[Enum.GetValues(typeof(Flag)).Length]; 
        
        // Sets the boolean in the bool array corresponding to the specified flag to "True"
        public void SetFlag(Flag flag) => Flags[(int) flag] = true;
        
        // Resets the boolean in the bool array corresponding to the specified flag to "False"
        public void ResetFlag(Flag flag) => Flags[(int) flag] = false;
        
        // Returns whether the boolean in the bool array corresponding to the specified flag is "true" or not. 
        public bool IsFlagSet(Flag flag) => Flags[(int) flag];
        
        // Creates an int array, the size of which is equal to the amount of Var Enum values
        public int[] Vars = new int[Enum.GetValues(typeof(Var)).Length];
        
        // Returns the int that corresponds to the specified Var value
        public int GetVar(Var var) => Vars[(int) var];
        
        // Sets the int at the index of the specified Var value
        public void SetVar(Var var, int value) => Vars[(int)var] = value;
    }
}
