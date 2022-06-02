using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

    public class DoorSystem : MonoBehaviour
    {
        public bool IsOpen;
        public string OpenName;
        public string CloseName;
        public Outline Outline;

        private void Awake()
        {
            Outline = GetComponent<Outline>();
        }

        private void Start()
        {
            Outline.enabled = false;
        }


        public void InitializeDoor()
        {
            EscapeRoom.Instance.OpenCloseButton.gameObject.SetActive(true);
            EscapeRoom.Instance.OpenCloseButton.GetComponentInChildren<TextMeshProUGUI>().text = IsOpen ? CloseName : OpenName;
        }

        public void EnabledOutline(bool enabled)
        {
            Outline.enabled = enabled;
        }

    }

