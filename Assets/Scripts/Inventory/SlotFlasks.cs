using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotFlasks : MonoBehaviour
    {
        public Image SlotImage;
        public TextMeshProUGUI AmountText;
        public bool CanUse;
        
        public void SetSlotFlask(bool value, Sprite sprite, int amount)
        {
            SlotImage.gameObject.SetActive(value);
            CanUse = value;
            SlotImage.sprite = sprite;
            AmountText.text = value ? amount.ToString() : 0.ToString();
        }

    }

