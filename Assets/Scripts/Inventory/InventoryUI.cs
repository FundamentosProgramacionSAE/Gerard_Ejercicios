using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
    {
        public GameObject SlotPrefab;

        private void OnEnable()
        {
            InventorySystem.Instance.OnInventoryChanged += OnInventoryChanged;
        }

        private void OnDisable()
        {
            InventorySystem.Instance.OnInventoryChanged -= OnInventoryChanged;

        }

        private void OnInventoryChanged()
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
        }

    }

