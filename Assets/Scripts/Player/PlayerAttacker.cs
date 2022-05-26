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
        
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;
        private string startCombo;
        
        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OHLightAttack1, true);
            LastAttack = weapon.OHLightAttack1;
            startCombo = weapon.OHLightAttack1;
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.ComboFlag)
            {
                animatorHandler.Animator.SetBool("canCombo", false);
                if (LastAttack == weapon.OHLightAttack1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OHLightAttack2, true);
                    LastAttack = weapon.OHLightAttack2;
                    return;
                }
                if (LastAttack == weapon.OHLightAttack2)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OHLightAttack3, true);
                    LastAttack = weapon.OHLightAttack3;
                    return;
                }
                if (LastAttack == weapon.OHLightAttack3)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OHLightAttack4, true);
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
            animatorHandler.PlayTargetAnimation(weapon.AbilityAttack2, true);
            LastAttack = weapon.AbilityAttack2;
        }
        public void HandleAbilityAttack3(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.AbilityAttack3, true);
            LastAttack = weapon.AbilityAttack3;
        }
        public void HandleAbilityAttack4(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.AbilityAttack4, true);
            LastAttack = weapon.AbilityAttack4;
        }
    }
}


