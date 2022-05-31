using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Manager;
using Inventory.Item;
using Player.Canvas;
using Player.Locomotion;
using Player.Manager;
using UnityEngine;

namespace Inventory
{
    public class PlayerWeaponInventory : MonoBehaviour
    {
        public WeaponItem RightWeapon;
        public WeaponItem LeftWeapon;
        public InventorySystem InventorySystem;
        
        private WeaponSlotManager weaponSlotManager;

        private WeaponItem _weaponReferenceRight;
        private WeaponItem _weaponReferenceLeft;

        private AbilityManager _abilityManager;
        private PlayerCanvas _playerCanvas;
        private AnimatorHandler _animatorHandler;
        private PlayerManager _playerManager;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            InventorySystem = GetComponentInChildren<InventorySystem>();
            _abilityManager = GetComponent<AbilityManager>();
            _playerCanvas = GetComponentInChildren<PlayerCanvas>();
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _playerManager = GetComponent<PlayerManager>();
        }
        
        

        private void Start()
        {
            Initialized();
        }

        public void UnEquip()
        {
            RightWeapon = null;
            LeftWeapon = null;
            _weaponReferenceRight = null;
            _weaponReferenceLeft = null;
            _animatorHandler.Animator.SetBool("isReposeWeapon", true);
            _playerManager.IsReposeWeapon = true;
            weaponSlotManager.UnEquip();
            
            _abilityManager.UnEquip();
            _playerCanvas.InitializedAbilities();
        }
        public void Initialized()
        {
            _weaponReferenceRight = null;
            _weaponReferenceLeft = null;
            foreach (var item in InventorySystem.ItemsDictionary)
            {
                print(item.Key.DisplayName);
                _weaponReferenceRight = item.Key as WeaponItem;

                if (_weaponReferenceRight != null)
                {
                    if (_weaponReferenceRight.IsUsed)
                    {
                        break;
                    }
                    else
                    {
                        _weaponReferenceRight = null;
                    }
                }

            }
            
            if(_weaponReferenceRight == null) return;
            
            if (_weaponReferenceRight.IsDualWeapon == false)
            {
                foreach (var item in InventorySystem.ItemsDictionary)
                {
                    print(item.Key.DisplayName);
                    _weaponReferenceLeft = item.Key as WeaponItem;

                    if (_weaponReferenceLeft != null)
                    {
                        if (_weaponReferenceLeft.IsUsed)
                        {
                            break;
                        }
                        else
                        {
                            _weaponReferenceLeft = null;
                        }
                    }

                }
            }

            

            
            if (_weaponReferenceRight.IsDualWeapon)
            {
                RightWeapon = _weaponReferenceRight;
                LeftWeapon = _weaponReferenceRight;
            }
            else
            {
                RightWeapon = _weaponReferenceRight;
                LeftWeapon = _weaponReferenceLeft;
            }
            weaponSlotManager.LoadWeaponSlot(RightWeapon,false);
            weaponSlotManager.LoadWeaponSlot(LeftWeapon,true);
            _animatorHandler.Animator.SetBool("isReposeWeapon", false);
            _playerManager.IsReposeWeapon = false;
            _abilityManager.Initialized();
            _playerCanvas.InitializedAbilities();
        }
        
    }
}


