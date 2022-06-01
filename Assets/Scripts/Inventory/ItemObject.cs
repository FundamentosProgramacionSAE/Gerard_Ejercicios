using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
    {
        public ItemData ItemData;
        public List<ItemRequirement> Requirements;
        public bool RemoveRequirementsOnPickup;
        public UnityEvent OnGetItem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPickUpItem();
            }
        }

        public void OnPickUpItem()
        {
            if (MeetsRequirements())
            {
                //if (ExceedItems()) return;
                
                if (RemoveRequirementsOnPickup)
                {
                    RemoveRequirements();
                }
                InventorySystem.Instance.Add(ItemData);
                
                OnGetItem?.Invoke();

                //Destroy(gameObject);

            }

        }

        public void RestartItem()
        {
            gameObject.SetActive(true);
        }

        private bool MeetsRequirements()
        {
            foreach (var requirement in Requirements)
            {
                if (!requirement.HasRequirement()) return false;
            }

            return true;
        }
        
        private bool ExceedItems()
        {
            foreach (var item in InventorySystem.Instance.ItemsDictionary)
            {
                if (item.Key == ItemData)
                {
                    if (item.Value.StackSize >= item.Key.MaxAmount) return true;
                }

            }

            return false;
        }

        private void RemoveRequirements()
        {
            foreach (var requirement in Requirements)
            {
                for (int i = 0; i < requirement.Amount; i++)
                {
                        InventorySystem.Instance.Remove(requirement.ItemData);
                }
            }
        }

    }

