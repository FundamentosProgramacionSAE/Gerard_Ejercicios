using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TooltipManager
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI HeaderText;
        public TextMeshProUGUI ContentText;
        public LayoutElement LayoutElement;
        public int CharacterWrapLimit;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                HeaderText.gameObject.SetActive(false);
            }
            else
            {
                HeaderText.gameObject.SetActive(true);
                HeaderText.text = header;
            }

            ContentText.text = content;
            
            int headerLength = HeaderText.text.Length;
            int contentLenght = ContentText.text.Length;

            LayoutElement.enabled =
                headerLength > CharacterWrapLimit || contentLenght > CharacterWrapLimit;
        }
        
    }
}


