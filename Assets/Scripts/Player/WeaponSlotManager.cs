using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;

namespace Inventory
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot LeftHandSlot;
        private WeaponHolderSlot RightHandSlot;
        
        private WeaponHolderSlot RestLeftHandSlot;
        private WeaponHolderSlot RestRightHandSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            
            foreach (var weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.IsLeftHandSlot && weaponSlot.isRest == false)
                {
                    LeftHandSlot = weaponSlot;
                }
                else if (weaponSlot.IsRightHandSlot && weaponSlot.isRest == false)
                {
                    RightHandSlot = weaponSlot;
                }
                else if (weaponSlot.IsLeftHandSlot && weaponSlot.isRest)
                {
                    RestLeftHandSlot = weaponSlot;
                }
                else if (weaponSlot.IsRightHandSlot && weaponSlot.isRest)
                {
                    RestRightHandSlot = weaponSlot;
                }
            }
        }
        
        public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                LeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.UnloadWeapon();
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                RightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.UnloadWeapon();
                LoadRightWeaponDamageCollider();
            }
        }


        public void OnEquipWeapon(WeaponItem weaponItem)
        {
            if (LeftHandSlot != null)
            {
                LeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.UnloadWeapon();
                LoadLeftWeaponDamageCollider();
            }

            if (RightHandSlot != null)
            {
                RightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.UnloadWeapon();
                LoadRightWeaponDamageCollider();
            }
        }
        
        public void OnUnequipWeapon(WeaponItem weaponItem)
        {
            if (LeftHandSlot != null)
            {
                LeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.LoadWeaponModel(weaponItem);
                LeftHandSlot.UnloadWeapon();
            }

            if (RightHandSlot != null)
            {
                RightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.LoadWeaponModel(weaponItem);
                RightHandSlot.UnloadWeapon();
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = LeftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = RightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void OpenAllWeaponDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
            rightHandDamageCollider.EnableDamageCollider();
        }
        
        private void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        
        private void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        private void CloseAllWeaponDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }
        private void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        
        private void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
        

        #endregion

    }
}


