using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            WeaponItem weaponItem = ItemData as WeaponItem;

            bool isDual = weaponItem.IsDualWeapon;
            if (weaponItem != null)
            {
                weaponItem.IsUsed = !weaponItem.IsUsed;

                if (weaponItem.IsUsed) EventSystem.Instance.OnUseWeapon(weaponItem,weaponItem,isDual);
                else EventSystem.Instance.OnUnEquipWeapon();
            }
        }
    }
}

