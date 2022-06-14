using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Manager;
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
        public LayerMask CollisionMask;
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
                var enemy = GetComponentInParent<EnemyManager>();

                if (enemy.IsAreaDamage == false)
                {
                    if (ApplyDamageToPlayer(collider)) return;
                }
                else
                {
                    
                }

            }
            if (collider.CompareTag("Enemy"))
            {
                ApplyDamageToEnemy(collider);
            }
        }



        public void AreaDamage(int radius)
        {
            var colliders = Physics.OverlapSphere(transform.position, radius, CollisionMask);
            
            foreach (var character in colliders)
            {
                if (character != null)
                {
                    if (character.CompareTag("Player"))
                    {
                        if (ApplyDamageToPlayer(character.GetComponent<Collider>())) return;
                    }
                    
                    if (character.CompareTag("Enemy"))
                    {
                        if (ApplyDamageToPlayer(character.GetComponent<Collider>())) return;
                    }
                }
            }
        }

        private void ApplyDamageToEnemy(Collider collider) 
        {
            EnemyStats enemyStats = collider.GetComponent<EnemyStats>();

            float directionHitFrom = (Vector3.SignedAngle(PlayerManager.Instance.transform.forward,
                enemyStats.GetComponent<EnemyManager>().transform.forward, Vector3.up));
            ChooseWichDirectionDamage(directionHitFrom);

            if (enemyStats == null) return;


            ApplyDamageAdded();
            var damagePercentToAdd = (PlayerManager.Instance.GetComponent<AbilityManager>().CurrentAbility
                .DamagePercentToAdd * WeaponDamage) / 100;
            WeaponDamage += PlayerManager.Instance.PlayerStats.DamageToAdd + damagePercentToAdd;
            float physicalDamageAfterDefense = (WeaponDamage * WeaponDamage) / (WeaponDamage + enemyStats.Defense);
            //Debug.LogError(physicalDamageAfterDefense + "/" + WeaponDamage);
            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterDefense), _currentDamageAnimation);
        }
        private bool ApplyDamageToPlayer(Collider collider) 
        {
            PlayerStats playerStats = collider.GetComponent<PlayerStats>();
            CharacterManager enemyCharacterManager = collider.GetComponent<CharacterManager>();
            var shield = playerStats.GetComponent<PlayerWeaponInventory>().LeftWeapon;
            var enemy = GetComponentInParent<EnemyManager>();

            ApplyDamageAdded();
            var damagePercentToAdd = (enemy.CurrentAttack.DamagePercentToAdd * WeaponDamage) / 100;
            WeaponDamage += enemy.EnemyStats.DamageToAdd + damagePercentToAdd;


            Vector3 directionEnemyToPlayer =
                (enemy.gameObject.transform.position - playerStats.gameObject.transform.position).normalized;
            var angleHit = Vector3.Dot(playerStats.gameObject.transform.forward, directionEnemyToPlayer);

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.IsBlocking && shield != null && angleHit >= 0)
                {
                    float physicalDamageAfterBlock =
                        WeaponDamage - (WeaponDamage * shield.PhysicalDamageAbsorption.GetValueFromRatio()) / 100;

                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                        return true;
                    }
                }
            }

            if (playerStats == null) return true;

            float physicalDamageAfterDefense = (WeaponDamage * WeaponDamage) / (WeaponDamage + playerStats.Defense);
            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterDefense));
            return false;
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


