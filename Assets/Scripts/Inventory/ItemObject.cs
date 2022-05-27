using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
    {
        public ItemData ItemData;
        public List<ItemRequirement> Requirements;
        public bool RemoveRequirementsOnPickup;

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
                if (RemoveRequirementsOnPickup)
                {
                    RemoveRequirements();
                }
                InventorySystem.Instance.Add(ItemData);

                Destroy(gameObject);

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

        private void RemoveRequirements()
        {
            foreach (var requirement in Requirements)
            {
                for (int i = 1; i <= requirement.Amount; i++)
                {
                    InventorySystem.Instance.Remove(requirement.ItemData);
                }
            }
        }

    }

