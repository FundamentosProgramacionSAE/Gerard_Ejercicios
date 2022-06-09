using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotFlasks : MonoBehaviour
    {
        public Image SlotImage;
        public TextMeshProUGUI AmountText;
        public bool CanUse;
        public SlotItem SlotItem;
        
        public void SetSlotFlask(bool value, Sprite sprite, int amount, SlotItem slotItem)
        {
            SlotImage.gameObject.SetActive(value);
            SlotItem = slotItem != null ? slotItem : null;
            CanUse = value;
            SlotImage.sprite = sprite;
            AmountText.text = value ? amount.ToString() : 0.ToString();
        }

    }

