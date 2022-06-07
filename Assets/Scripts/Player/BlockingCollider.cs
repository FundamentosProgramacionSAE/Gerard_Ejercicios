using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;

namespace Player.Stats
{
    public class BlockingCollider : MonoBehaviour
    {
        public float BlockingPhysicalDamage;
        
        internal BoxCollider _blockingCollider;


        private void Awake()
        {
            _blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorption(WeaponItem weapon)
        {
            if (weapon != null)
            {
                BlockingPhysicalDamage = weapon.PhysicalDamageAbsorption.GetValueFromRatio();
            }
        }

        public void EnableBlockingCollider()
        {
            _blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            _blockingCollider.enabled = false;
        }
    }
    
}

