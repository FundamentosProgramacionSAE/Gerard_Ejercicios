using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using Player.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponHolderSlot LeftHandSlot;
        public WeaponHolderSlot RightHandSlot;
        
        public WeaponHolderSlot RestLeftHandSlot;
        public WeaponHolderSlot RestRightHandSlot;

        internal DamageCollider leftHandDamageCollider;
        internal DamageCollider rightHandDamageCollider;

        private PlayerWeaponInventory _playerWeaponInventory;
        private Animator animator;
        private PlayerManager _playerManager;

        private void Awake()
        {
            _playerWeaponInventory = GetComponentInParent<PlayerWeaponInventory>();
            _playerManager = GetComponentInParent<PlayerManager>();
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

        public void LoadBothWeaponOnSlot()
        {
            LoadWeaponSlot(_playerWeaponInventory.RightWeapon, false);
            LoadWeaponSlot(_playerWeaponInventory.LeftWeapon, true);
        }
        public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                LeftHandSlot.CurrentWeaponItem = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.LoadWeaponModel(weaponItem);
                RestLeftHandSlot.UnloadWeapon();
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                RightHandSlot.CurrentWeaponItem = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.LoadWeaponModel(weaponItem);
                RestRightHandSlot.UnloadWeapon();
                LoadRightWeaponDamageCollider();
            }
        }


        public void OnEquipWeapon()
        {
            if (LeftHandSlot != null)
            {
                LeftHandSlot.LoadWeaponModel(_playerWeaponInventory.LeftWeapon);
                RestLeftHandSlot.LoadWeaponModel(_playerWeaponInventory.LeftWeapon);
                RestLeftHandSlot.UnloadWeapon();
                LoadLeftWeaponDamageCollider();
            }

            if (RightHandSlot != null)
            {
                RightHandSlot.LoadWeaponModel(_playerWeaponInventory.RightWeapon);
                RestRightHandSlot.LoadWeaponModel(_playerWeaponInventory.RightWeapon);
                RestRightHandSlot.UnloadWeapon();
                LoadRightWeaponDamageCollider();
            }
        }

        public void UnEquip()
        {
            Destroy(LeftHandSlot.CurrentWeaponModel);
            Destroy(RightHandSlot.CurrentWeaponModel);
            Destroy(RestLeftHandSlot.CurrentWeaponModel);
            Destroy(RestRightHandSlot.CurrentWeaponModel);
        }

        public void UnEquipLeft()
        {
            Destroy(LeftHandSlot.CurrentWeaponModel);
            Destroy(RestLeftHandSlot.CurrentWeaponModel);
        }
        
        public void OnRestWeapon()
        {
            if (LeftHandSlot != null)
            {
                LeftHandSlot.LoadWeaponModel(_playerWeaponInventory.LeftWeapon);
                RestLeftHandSlot.LoadWeaponModel(_playerWeaponInventory.LeftWeapon);
                LeftHandSlot.UnloadWeapon();
            }

            if (RightHandSlot != null)
            {
                RightHandSlot.LoadWeaponModel(_playerWeaponInventory.RightWeapon);
                RestRightHandSlot.LoadWeaponModel(_playerWeaponInventory.RightWeapon);
                RightHandSlot.UnloadWeapon();
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            if(LeftHandSlot.CurrentWeaponModel != null) leftHandDamageCollider = LeftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        private void LoadRightWeaponDamageCollider()
        {
            if(RightHandSlot.CurrentWeaponModel != null) rightHandDamageCollider = RightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            if (_playerManager.IsUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            if (_playerManager.IsUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }

        }

        public void CloseDamageCollider()
        {
            if(rightHandDamageCollider != null) rightHandDamageCollider.DisableDamageCollider();
            if(leftHandDamageCollider != null)leftHandDamageCollider.DisableDamageCollider();
        }
        

        #endregion

    }
}


