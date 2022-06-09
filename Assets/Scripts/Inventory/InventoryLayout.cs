using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using Player.Canvas;
using UnityEngine;

    public class InventoryLayout : MonoBehaviour
    {
        public bool HasOccupied;
        public SlotItem SlotItem;
        public int Position;
        public SlotFlasks SlotFlask;
        

        public void RemoveSlot()
        {
            SlotItem = null;
            HasOccupied = false;
            
            if(!SlotFlask) return;
            
            SetFlaskInventory(false, null,0, null);

        }

        public void SetSlot(SlotItem slotItem)
        {
            HasOccupied = true;
            SlotItem = slotItem;


            var flask = SlotItem.ItemData as FlaskItem;
            
            if(!flask || !SlotFlask) return;
            
            SetFlaskInventory(true, flask.Icon, SlotItem.Item.StackSize, flask);
            
        }

        public void SetFlaskInventory(bool value, Sprite Icon, int StackSize, FlaskItem flaskItem)
        {
            if(!SlotFlask) return;
            SlotFlask.SetSlotFlask(value, Icon, StackSize, SlotItem);
        }
        
    }

