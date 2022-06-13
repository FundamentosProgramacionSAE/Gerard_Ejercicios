using System;
using System.Collections;
using System.Collections.Generic;
using AI.Manager;
using AI.Stats;
using Player.Manager;
using Player.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.Item
{
    public class DamageCollider : MonoBehaviour
    {
        private CapsuleCollider damageCollider;
        public int WeaponDamage;
        public WeaponItem WeaponItem;
        private string _currentDamageAnimation;
        
        private void Awake()
        {
            damageCollider = GetComponent<CapsuleCollider>();
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
                var shield = playerStats.GetComponent<PlayerWeaponInventory>().LeftWeapon;
                var enemy = GetComponentInParent<EnemyManager>();
                
                ApplyDamageAdded();
                WeaponDamage += enemy.EnemyStats.DamageToAdd;

                Vector3 directionEnemyToPlayer =
                    (enemy.gameObject.transform.position - playerStats.gameObject.transform.position).normalized;
                var angleHit = Vector3.Dot(playerStats.gameObject.transform.forward, directionEnemyToPlayer);
                
                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.IsBlocking && shield != null && angleHit>=0)
                    {
                        float physicalDamageAfterBlock =
                            WeaponDamage - (WeaponDamage * shield.PhysicalDamageAbsorption.GetValueFromRatio()) / 100;

                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }
                
                if(playerStats == null) return;
                
                float physicalDamageAfterDefense =   (WeaponDamage * WeaponDamage) / (WeaponDamage + playerStats.Defense);
                playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterDefense));
            }
            if (collider.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
                
                float directionHitFrom = (Vector3.SignedAngle(PlayerManager.Instance.transform.forward,
                    EnemyManager.Instance.transform.forward, Vector3.up));
                ChooseWichDirectionDamage(directionHitFrom);
                
                if(enemyStats == null) return;


                ApplyDamageAdded();
                WeaponDamage += PlayerManager.Instance.PlayerStats.DamageToAdd;
                float physicalDamageAfterDefense = (WeaponDamage * WeaponDamage) / (WeaponDamage + enemyStats.Defense);
                Debug.LogError(physicalDamageAfterDefense + "/" + WeaponDamage);
                enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterDefense), _currentDamageAnimation);
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

        private void ChooseWichDirectionDamage(float direction)
        {
            if (direction >= 145 && direction <= 180)
            {
                _currentDamageAnimation = "Damage_01";
            }
            else if (direction <= -145 && direction >= 180)
            {
                _currentDamageAnimation = "Damage_01";
            }
            else if (direction >= -45 && direction <= 45)
            {
                _currentDamageAnimation = "Damage_02";
            }
            else if (direction >= -144 && direction <= -45)
            {
                _currentDamageAnimation = "Damage_03";
            }
            else if (direction >= 45 && direction <= 144)
            {
                _currentDamageAnimation = "Damage_04";
            }
            else
            {
                _currentDamageAnimation = "Damage_01";
            }
        }
    }
}


