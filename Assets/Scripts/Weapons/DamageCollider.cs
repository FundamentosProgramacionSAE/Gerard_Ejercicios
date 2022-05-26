using System;
using System.Collections;
using System.Collections.Generic;
using AI.Stats;
using Player.Stats;
using UnityEngine;

namespace Inventory.Item
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        public int WeaponDamage;


        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }


        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerStats playerStats = collider.GetComponent<PlayerStats>();
                
                if(playerStats == null) return;
                
                playerStats.TakeDamage(WeaponDamage);
            }
            
            if (collider.CompareTag("Enemy"))
            {
                EnemyStats playerStats = collider.GetComponent<EnemyStats>();
                
                if(playerStats == null) return;
                
                playerStats.TakeDamage(WeaponDamage);
            }
        }
    }
}


