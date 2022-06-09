using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using  Sirenix.OdinInspector;

public class InventorySystem : SerializedMonoBehaviour
{
    public event Action<InventoryItem> OnAddItem;
    public event Action<InventoryItem> OnAddStackItem;
    public event Action<InventoryItem> OnRemoveItem;
    public event Action<InventoryItem> OnRemoveStackItem;
    public event Action OnStartInventory;
    public event Action OnRestartInventory;
    public event Action OnAddGold;
    public static InventorySystem Instance { get; private set; }

    
    public Dictionary<ItemData, InventoryItem> ItemsDictionary = new Dictionary<ItemData, InventoryItem>();
    public int Gold;


    private PlayerWeaponInventory _playerWeaponInventory;
    private InventoryUI _inventoryUI;


    public InventorySystem()
    {
        ItemsDictionary = new Dictionary<ItemData, InventoryItem>();
    }
    
    private void Awake()
    {
        Instance = this;
        _playerWeaponInventory = GetComponentInParent<PlayerWeaponInventory>();
        _inventoryUI = GetComponentInChildren<InventoryUI>();

    }


    public void Initialized()
    {

        if (ES3.KeyExists("Inventory") == false)
        {
            ItemsDictionary = new Dictionary<ItemData, InventoryItem>();
        }
        StartInventory();
        OnAddGold?.Invoke();
        _playerWeaponInventory.Initialized();
        _inventoryUI.Initialized();
    }


    public InventoryItem Get(ItemData referenceData)
    {
        if (ItemsDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }

        return null;
    }

    public void Add(ItemData referenceData)
    {
        if (ItemsDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
            OnAddStackItem?.Invoke(value);

        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            ItemsDictionary.Add(referenceData, newItem);
            OnAddItem?.Invoke(newItem);
        }

    }

    private void Update()
    {
        if (Keyboard.current[Key.F1].wasPressedThisFrame)
        {
            RestartInventory();
        }
    }

    public void RestartInventory()
    {
        OnRestartInventory?.Invoke();
    }

    public void StartInventory()
    {
        OnStartInventory?.Invoke();
    }

    public void Remove(ItemData referenceData, bool removeCompleted = false)
    {
        if (ItemsDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            if (removeCompleted)
            {
                ItemsDictionary.Remove(referenceData);
                OnRemoveItem?.Invoke(value);
            }
            else
            {
                value.RemoveFromStack();
                OnRemoveStackItem?.Invoke(value);

                if (value.StackSize == 0)
                {
                    ItemsDictionary.Remove(referenceData);
                    OnRemoveItem?.Invoke(value);
                }
            }

        }
    }
    
}




