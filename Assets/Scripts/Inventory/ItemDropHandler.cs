using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using Player.Canvas;
using Player.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public bool OnlyFlask;
    public bool IsItem;
    public bool IsDropZone;
    public void OnDrop(PointerEventData eventData)
    {
        if (IsDropZone)
        {
            var itemData = eventData.pointerDrag.GetComponent<SlotItem>().ItemData;
            if(itemData as WeaponItem != null) return;
            InventorySystem.Instance.Remove(itemData, true);
        }
        else
        {                


            if (IsItem)
            {
                var slotItemDropped = eventData.pointerDrag.GetComponent<SlotItem>();
                var slotItem = GetComponent<SlotItem>();
                var flask = slotItemDropped.ItemData as FlaskItem;

                if (slotItem.InventoryLayout.GetComponent<ItemDropHandler>().OnlyFlask)
                {
                    if ( flask == null)
                    {
                        Debug.LogWarning("No puedes poner objetos que no sean pociones");
                        return;
                    }
                }

                var slotLayoutDropped = slotItemDropped.InventoryLayout;
                var slotLayout = slotItem.InventoryLayout;

                slotItem.InventoryLayout = slotLayoutDropped;
                slotItemDropped.InventoryLayout = slotLayout;
            
                slotItem.InventoryLayout.SetSlot(slotItem);
                slotItemDropped.InventoryLayout.SetSlot(slotItemDropped);
            
                eventData.pointerDrag.GetComponent<ItemDragHandler>().startPosition =
                    slotLayout.GetComponent<RectTransform>().localPosition;
                GetComponent<RectTransform>().localPosition = slotLayoutDropped.GetComponent<RectTransform>().localPosition;

                slotItem.ItemData.Position = slotItem.InventoryLayout.Position;
                slotItemDropped.ItemData.Position = slotItemDropped.InventoryLayout.Position;

            }
            else
            {

                var slotItem = eventData.pointerDrag.GetComponent<SlotItem>();
                var inventoryLayout = GetComponent<InventoryLayout>();
                var flask = slotItem.ItemData as FlaskItem;

                if (OnlyFlask)
                {
                    if (eventData.pointerDrag.GetComponent<SlotItem>().ItemData as FlaskItem == null) return;
                    
                }

                eventData.pointerDrag.GetComponent<ItemDragHandler>().startPosition =
                    GetComponent<RectTransform>().localPosition;

                slotItem.InventoryLayout.HasOccupied = false;
                slotItem.InventoryLayout.RemoveSlot(); 
                slotItem.InventoryLayout = inventoryLayout;
                inventoryLayout.SetSlot(slotItem);
            
                slotItem.ItemData.Position = inventoryLayout.Position;
            }
        }
        

    }
    
}

