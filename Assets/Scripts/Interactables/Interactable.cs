using System;
using System.Collections;
using System.Collections.Generic;
using Player.Manager;
using TMPro;
using UnityEngine;

namespace InteractableItems
{
    public class Interactable : MonoBehaviour
    {
        public bool IsCollected;
        public string InteractableText;
        
        public virtual void Interact(PlayerManager playerManager)
        {
            print("Has interactuado!");
        }
    }
}


