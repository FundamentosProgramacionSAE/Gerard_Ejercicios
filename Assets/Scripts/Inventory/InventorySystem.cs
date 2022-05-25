using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour ,IDataPersistence
    {

        public event Action OnInventoryChanged;
        public static InventorySystem Instance { get; private set; }
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
                
            }
            else
            {
                InventoryItem newItem = new InventoryItem(referenceData);
                ItemsDictionary.Add(referenceData,newItem);
            }

            OnInventoryChanged?.Invoke();
        }

        public void RestartInventory()
        {
            ItemsDictionary.Clear();
            OnInventoryChanged?.Invoke();
        }

        public void Remove(ItemData referenceData)
        {
            if (ItemsDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                value.RemoveFromStack();

                if (value.StackSize == 0)
                {
                    ItemsDictionary.Remove(referenceData);
                }
            }

            OnInventoryChanged?.Invoke();
        }

        public void LoadData(GameData data)
        {
            //ItemsDictionary = data.ItemsDictionary;

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
            OnInventoryChanged?.Invoke();
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




