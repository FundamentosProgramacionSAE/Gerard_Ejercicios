using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;

    [Serializable]
    public class InventoryItem
    {
        public ItemData Data;
        public int StackSize;


        public InventoryItem(ItemData source)
        {
            Data = source;
            AddToStack();
        }
        

        public void AddToStack()
        {
            StackSize += Data.RandomAmounts.GetValueFromRatio();
            StackSize = Mathf.Clamp(StackSize, 0, Data.MaxAmount);
        }

        public void RemoveFromStack() => StackSize--;
    }

