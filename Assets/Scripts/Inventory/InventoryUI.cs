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
            PlayerCanvas.SetPositionsInventory();
        }

        private void OnEnable()
        {
            InventorySystem.Instance.OnAddItem += OnAddItem;
            InventorySystem.Instance.OnAddStackItem += OnAddStackItem;
            InventorySystem.Instance.OnRemoveItem += OnRemoveItem;
            InventorySystem.Instance.OnRemoveStackItem += OnRemoveStackItem;
            InventorySystem.Instance.OnStartInventory += OnStartInventory;
            InventorySystem.Instance.OnRestartInventory += OnRestartInventory;

        }

        private void OnDisable()
        {
            InventorySystem.Instance.OnAddItem -= OnAddItem;
            InventorySystem.Instance.OnAddStackItem -= OnAddStackItem;
            InventorySystem.Instance.OnRemoveItem -= OnRemoveItem;
            InventorySystem.Instance.OnRemoveStackItem -= OnRemoveStackItem;
            InventorySystem.Instance.OnStartInventory -= OnStartInventory;
            InventorySystem.Instance.OnRestartInventory -= OnRestartInventory;

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
            RemoveInventorySlot(item);
        }



        private void OnRemoveStackItem(InventoryItem item)
        {
            SetSlotItem(item);
        }
        

        private void OnStartInventory()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            
            DrawInventory();
        }

        private void OnRestartInventory()
        {
            var inventorySystem = InventorySystem.Instance;
            foreach (var item in inventorySystem.ItemsDictionary)
            {
                item.Key.Position = -1;
            }
            
            inventorySystem.ItemsDictionary.Clear();
            Items.Clear();
            PlayerCanvas.SetDefaultLayouts();
            
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }

        private void DrawInventory()
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
            
            if (slot.ItemData.Position == -1)
            {
                foreach (var layout in PlayerCanvas.Layouts)
                {
                    if (layout.HasOccupied) continue;
                
                    obj.transform.localPosition = layout.GetComponent<RectTransform>().localPosition;
                    layout.SetSlot(slot);
                    slot.InventoryLayout = layout;
                    slot.ItemData.Position = layout.Position;
                    return;


                }
            }
            else
            {
                obj.transform.localPosition = PlayerCanvas.Layouts[slot.ItemData.Position].GetComponent<RectTransform>().localPosition;
                PlayerCanvas.Layouts[slot.ItemData.Position].SetSlot(slot);
                slot.InventoryLayout = PlayerCanvas.Layouts[slot.ItemData.Position];
                return;
            }

        }
        
        private void RemoveInventorySlot(InventoryItem item)
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

                    item.Data.Position = -1;
                    Destroy(Items[i].gameObject);
                    Items.RemoveAt(i);
                }
            }
        }
        
        private void SetSlotItem(InventoryItem item)
        {
            foreach (var slotItem in Items)
            {
                if (slotItem.ItemData == item.Data)
                {
                    slotItem.Set(item);
                }
            }
        }

    }

