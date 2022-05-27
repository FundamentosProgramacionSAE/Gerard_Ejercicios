using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooltipManager
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem Instance;

        public Tooltip Tooltip;


        private void Awake()
        {
            Instance = this;
            Hide();
        }


        public static void Show(string content, string header = "")
        {
            Instance.Tooltip.SetText(content, header);
            Instance.Tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            Instance.Tooltip.gameObject.SetActive(false);
        }
    }
}


