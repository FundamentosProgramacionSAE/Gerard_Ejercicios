using System;
using System.Collections;
using System.Collections.Generic;
using InteractableItems;
using Player.Manager;
using UnityEngine;

namespace InteractableItems
{
    public class Chest : Interactable
    {
        public Transform PlayerStandingPosition;
        public ItemObject ItemObject;
        public GameObject SpawnCollectedVFX;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }


        public override void Interact(PlayerManager playerManager)
        {
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            
            
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            playerManager.OpenChestInteraction(PlayerStandingPosition, ItemObject.ItemData.DisplayName, ItemObject.ItemData.Icon);
            Instantiate(SpawnCollectedVFX, transform.position, Quaternion.identity);
            _animator.Play("Chest Open");
            ItemObject.OnPickUpItem();
        }
        
    }
    
}

