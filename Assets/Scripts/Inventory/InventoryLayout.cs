using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class InventoryLayout : MonoBehaviour
    {
        public bool HasOccupied;
        public SlotItem SlotItem;
        public int Position;
        
        public void RemoveSlot()
        {
            SlotItem = null;
            HasOccupied = false;
        }

        public void SetSlot(SlotItem slotItem)
        {
            HasOccupied = true;
            SlotItem = slotItem;
        }
        
    }

