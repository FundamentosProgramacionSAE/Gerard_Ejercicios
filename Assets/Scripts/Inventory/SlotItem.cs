using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public Image IconItem;
    public TextMeshProUGUI NameItem;
    public GameObject StackObj;
    public TextMeshProUGUI StackLabel;
    public ItemData ItemData;
    public InventoryLayout InventoryLayout;


    public void Set(InventoryItem item)
    {
        ItemData = item.Data;
        IconItem.sprite = item.Data.Icon;
        if(NameItem != null) NameItem.SetText(item.Data.DisplayName);

        print(item.StackSize);
        if (item.StackSize <= 1)
        {
            StackObj.SetActive(false);
            return;
        }
        else
        {
            StackObj.SetActive(true);
        }
        
        StackLabel.SetText(item.StackSize.ToString());
    }
    
}

