using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using Player.Input;
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
        
        private void Awake()
        {
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            inputHandler = GetComponent<InputHandler>();
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
    }
}


