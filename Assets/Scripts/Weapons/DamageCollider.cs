using System;
using System.Collections;
using System.Collections.Generic;
using AI.Stats;
using Player.Manager;
using Player.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.Item
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        public int WeaponDamage;
        public WeaponItem WeaponItem;

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
                CharacterManager enemyCharacterManager = collider.GetComponent<CharacterManager>();
                BlockingCollider shield = collider.transform.GetComponentInChildren<BlockingCollider>();
                
                ApplyDamageAdded();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.IsBlocking && shield != null)
                    {
                        float physicalDamageAfterBlock =
                            WeaponDamage - (WeaponDamage * shield.BlockingPhysicalDamage) / 100;

                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }
                
                if(playerStats == null) return;
                

                playerStats.TakeDamage(WeaponDamage);
            }
            
            if (collider.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
                
                if(enemyStats == null) return;

                ApplyDamageAdded();
                enemyStats.TakeDamage(WeaponDamage);
            }
        }

        private void ApplyDamageAdded()
        {
            WeaponDamage = WeaponItem.WeaponDamage + WeaponItem.DamageToAdd.GetValueFromRatio();
            
            var randomChance = Random.Range(0, 100);

            if (randomChance <= WeaponItem.CriticalRate)
            {
                // CRIT
                WeaponDamage = (int)(WeaponDamage * WeaponItem.CriticalDamageMultiplier);
            }
        }
    }
}


