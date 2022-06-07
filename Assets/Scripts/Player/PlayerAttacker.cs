using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using Managers;
using Player.Input;
using Player.Manager;
using UnityEngine;

namespace Player.Locomotion
{
    public class PlayerAttacker : MonoBehaviour
    {
        
        public string LastAttack;
        public float ComboDuration = .5f;
        
        private PlayerAnimatorManager _playerAnimatorManager;
        private InputHandler inputHandler;
        private string startCombo;
        private PlayerManager _playerManager;
        private PlayerEquipmentManager _playerEquipmentManager;
        private PlayerWeaponInventory _playerWeaponInventory;
        
        private void Awake()
        {
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            _playerEquipmentManager = GetComponentInChildren<PlayerEquipmentManager>();
            _playerWeaponInventory = GetComponent<PlayerWeaponInventory>();
            inputHandler = GetComponent<InputHandler>();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.OHLightAttack1, true);
            LastAttack = weapon.OHLightAttack1;
            startCombo = weapon.OHLightAttack1;
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.ComboFlag)
            {
                _playerAnimatorManager.Animator.SetBool("canCombo", false);
                if (LastAttack == weapon.OHLightAttack1)
                {
                    _playerAnimatorManager.PlayTargetAnimation(weapon.OHLightAttack2, true);
                    LastAttack = weapon.OHLightAttack2;
                    return;
                }
                if (LastAttack == weapon.OHLightAttack2)
                {
                    _playerAnimatorManager.PlayTargetAnimation(weapon.OHLightAttack3, true);
                    LastAttack = weapon.OHLightAttack3;
                    return;
                }
                if (LastAttack == weapon.OHLightAttack3)
                {
                    _playerAnimatorManager.PlayTargetAnimation(weapon.OHLightAttack4, true);
                    return;
                }
            }
            
        }

        public void ResetCombo()
        {
            LastAttack = startCombo;
        }

        public void HandleAbilityAttack2(WeaponItem weapon)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.AbilityAttack2, true);
            LastAttack = weapon.AbilityAttack2;
        }
        public void HandleAbilityAttack3(WeaponItem weapon)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.AbilityAttack3, true);
            LastAttack = weapon.AbilityAttack3;
        }
        public void HandleAbilityAttack4(WeaponItem weapon)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.AbilityAttack4, true);
            LastAttack = weapon.AbilityAttack4;
        }
        
        public void HandleBlock()
        {
            PerformBlockAction();
        }

        private void PerformBlockAction()
        {
            if(_playerManager.IsInteracting) return;
            if(_playerManager.IsBlocking || !_playerWeaponInventory.LeftWeapon) return;
            
            _playerManager.IsBlocking = true;
            _playerAnimatorManager.PlayTargetAnimation("Block",false, true);
            _playerEquipmentManager.OpenBlockingCollider();
        }
    }
}


