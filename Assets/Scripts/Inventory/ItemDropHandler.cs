using System.Collections;
using System.Collections.Generic;
using Player.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public bool IsItem;
    public bool IsDropZone;
    public void OnDrop(PointerEventData eventData)
    {
        if (IsDropZone)
        {
            InventorySystem.Instance.Remove(eventData.pointerDrag.GetComponent<SlotItem>().ItemData, true);
        }
        else
        {
            if (IsItem)
            {
                var slotItemDropped = eventData.pointerDrag.GetComponent<SlotItem>();
                var slotItem = GetComponent<SlotItem>();
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

