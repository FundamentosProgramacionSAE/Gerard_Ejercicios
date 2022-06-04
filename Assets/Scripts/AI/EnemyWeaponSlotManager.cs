using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using UnityEngine;

namespace AI.Manager
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {

        public WeaponItem RightHandWeapon;
        public WeaponItem LeftHandWeapon;
        
        private WeaponHolderSlot _rightHandSlot;
        private WeaponHolderSlot _leftHandSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightDamageCollider;


        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (var weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.IsLeftHandSlot && weaponSlot.isRest == false)
                {
                    _leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.IsRightHandSlot && weaponSlot.isRest == false)
                {
                    _rightHandSlot = weaponSlot;
                }
            }
        }

        private void Start()
        {
            LoadWeaponOnBothHands();
        }

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                _leftHandSlot.CurrentWeaponItem = weapon;
                _leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                _rightHandSlot.CurrentWeaponItem = weapon;
                _rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeaponOnBothHands()
        {
            if (RightHandWeapon != null)
            {
                LoadWeaponOnSlot(RightHandWeapon, false);
            }

            if (LeftHandWeapon != null)
            {
                LoadWeaponOnSlot(LeftHandWeapon, true);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                _leftHandDamageCollider = _leftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                _rightDamageCollider = _rightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        public void OpenDamageCollider()
        {
            _rightDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            _rightDamageCollider.DisableDamageCollider();
        }
    }
    
}

