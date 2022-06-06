using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using Player.Canvas;
using Player.Manager;
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
    
    public Image CooldownImage;
    public bool IsUsingItem;
    public float TimeToUse;

    private float _timeToReturn;
    private InventoryUI _inventoryUI;
    internal InventoryItem Item;

    private void Start()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void Set(InventoryItem item)
    {
        Item = item;
        ItemData = item.Data;
        IconItem.sprite = item.Data.Icon;
        if(NameItem != null) NameItem.SetText(item.Data.DisplayName);

        print(item.StackSize);
        
        if (InventoryLayout != null)
        {
            InventoryLayout.SetFlaskInventory(true, ItemData.Icon, Item.StackSize);
        }
        
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

    private void Update()
    {
        if (IsUsingItem && CooldownImage != null)
        {
            _timeToReturn -= Time.deltaTime;
            _timeToReturn = Mathf.Clamp(_timeToReturn, 0, TimeToUse);
            CooldownImage.fillAmount =
                (_timeToReturn - 0) / (TimeToUse - 0);
            if (_timeToReturn <= 0)
            {
                IsUsingItem = false;
            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            WeaponItem weaponItem = ItemData as WeaponItem;
            FlaskItem flaskItem = ItemData as FlaskItem;

            if (weaponItem != null)
            {
                bool isDual = weaponItem.IsDualWeapon;
                weaponItem.IsUsed = !weaponItem.IsUsed;

                if (weaponItem.IsUsed) EventSystem.Instance.OnUseWeapon(weaponItem,weaponItem,isDual);
                else EventSystem.Instance.OnUnEquipWeapon();
            }

            if (flaskItem != null)
            {
                if(IsUsingItem) return;
                EventSystem.Instance.OnHealPlayer(flaskItem);
                foreach (var item in _inventoryUI.Items)
                {
                    item.ResetConsumption(flaskItem.TimeToUse);
                }
            }
        }
    }

    private void ResetConsumption(float time)
    {
        if (ItemData as FlaskItem)
        {
            TimeToUse = time;
            _timeToReturn = time;
            IsUsingItem = true;
        }

    }
}

