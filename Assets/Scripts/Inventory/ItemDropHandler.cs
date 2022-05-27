using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<ItemDragHandler>().startPosition =
            GetComponent<RectTransform>().localPosition;
        eventData.pointerDrag.GetComponent<SlotItem>().InventoryLayout.HasOccupied = false;
        eventData.pointerDrag.GetComponent<SlotItem>().InventoryLayout.SlotItem.InventoryLayout =  GetComponent<InventoryLayout>();
        GetComponent<InventoryLayout>().SetSlot(eventData.pointerDrag.GetComponent<SlotItem>());
    }
}

