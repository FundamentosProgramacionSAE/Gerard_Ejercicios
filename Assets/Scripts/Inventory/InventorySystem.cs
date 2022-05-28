using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour ,IDataPersistence
    {

        public event Action<InventoryItem> OnAddItem;
        public event Action<InventoryItem> OnAddStackItem;
        public event Action<InventoryItem> OnRemoveItem;
        public event Action<InventoryItem> OnRemoveStackItem;
        public event Action OnStartInventory;
        public event Action OnRestartInventory;
        public static InventorySystem Instance { get; private set; }
        public List<GameObject> InventoryItems = new List<GameObject>();
        public SerializableDictionary<ItemData, InventoryItem> ItemsDictionary;


        private void Awake()
        {
            Instance = this;
            ItemsDictionary = new SerializableDictionary<ItemData, InventoryItem>();
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
                ItemsDictionary.Add(referenceData,newItem);
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

        public void Remove(ItemData referenceData)
        {
            if (ItemsDictionary.TryGetValue(referenceData, out InventoryItem value))
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

        public void LoadData(GameData data)
        {

            foreach (var item in data.ItemsDictionary)
            {
                foreach (var res in Resources.LoadAll<ItemData>("Items"))
                {
                    if (res.ID != item.Key.ToString()) continue;

                    InventoryItem newItem = new InventoryItem(res)
                    {
                        StackSize = int.Parse(item.Value)
                    };
                    ItemsDictionary.Add(res, newItem);
                }

            }
        }

        public void SaveData(GameData data)
        {
            //data.ItemsDictionary = ItemsDictionary;

            data.ItemsDictionary.Clear();
            foreach (var item in ItemsDictionary)
            {
                data.ItemsDictionary.Add(item.Key.ID, item.Value.StackSize.ToString());
            }
        }
    }




