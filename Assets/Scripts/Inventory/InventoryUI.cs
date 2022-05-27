using System;
using System.Collections;
using System.Collections.Generic;
using Player.Canvas;
using UnityEngine;

public class InventoryUI : MonoBehaviour
    {
        public GameObject SlotPrefab;
        public List<SlotItem> Items = new List<SlotItem>();
        public PlayerCanvas PlayerCanvas;


        private void Awake()
        {
            PlayerCanvas = GetComponentInParent<PlayerCanvas>();
        }

        private void OnEnable()
        {
            InventorySystem.Instance.OnAddItem += OnAddItem;
            InventorySystem.Instance.OnAddStackItem += OnAddStackItem;
            InventorySystem.Instance.OnRemoveItem += OnRemoveItem;
            InventorySystem.Instance.OnRemoveStackItem += OnRemoveStackItem;
            InventorySystem.Instance.OnStartInventory += OnStartInventory;
        }

        private void OnDisable()
        {
            InventorySystem.Instance.OnAddItem -= OnAddItem;
            InventorySystem.Instance.OnAddStackItem -= OnAddStackItem;
            InventorySystem.Instance.OnRemoveItem -= OnRemoveItem;
            InventorySystem.Instance.OnRemoveStackItem -= OnRemoveStackItem;
            InventorySystem.Instance.OnStartInventory -= OnStartInventory;

        }

        private void OnAddItem(InventoryItem item)
        {
            AddInventorySlot(item);
        }
        private void OnAddStackItem(InventoryItem item)
        {
            foreach (var slotItem in Items)
            {
                if (slotItem.ItemData == item.Data)
                {
                    slotItem.Set(item);
                }
            }
        }

        private void OnRemoveItem(InventoryItem item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].ItemData == item.Data)
                {
                    foreach (var layout in PlayerCanvas.Layouts)
                    {
                        if (layout.SlotItem == Items[i])
                        {
                            layout.RemoveSlot();
                        }
                    }
                    Destroy(Items[i].gameObject);
                    if(Items[i] == null) Items.RemoveAt(i);
                }
            }
        }

        private void OnRemoveStackItem(InventoryItem item)
        {
            foreach (var slotItem in Items)
            {
                if (slotItem.ItemData == item.Data)
                {
                    slotItem.Set(item);
                }
            }
        }
        

        private void OnStartInventory()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            
            DrawInventory();
        }

        public void DrawInventory()
        {
            foreach (var item in InventorySystem.Instance.ItemsDictionary)
            {
                AddInventorySlot(item.Value);
            }

        }

        public void AddInventorySlot(InventoryItem item)
        {
            GameObject obj = Instantiate(SlotPrefab);
            obj.transform.SetParent(transform,false);

            SlotItem slot = obj.GetComponent<SlotItem>();
            slot.Set(item);
            Items.Add(slot);
            
            foreach (var layout in PlayerCanvas.Layouts)
            {
                if (layout.HasOccupied) continue;
                obj.transform.localPosition = layout.GetComponent<RectTransform>().localPosition;
                layout.SetSlot(slot);
                slot.InventoryLayout = layout;
                return;


            }
        }

    }

