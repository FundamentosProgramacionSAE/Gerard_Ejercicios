using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TooltipManager
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Content;
        public string Header;
        public SlotItem SlotItem;

        private void Start()
        {
            Header = SlotItem.ItemData.DisplayName;
            Content = SlotItem.ItemData.InfoItem;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipSystem.Show(Content, Header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }
    }
}


