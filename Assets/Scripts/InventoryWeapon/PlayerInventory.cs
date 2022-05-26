using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public WeaponItem RightWeapon;
        public WeaponItem LeftWeapon;
        
        private WeaponSlotManager weaponSlotManager;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }


        private void Start()
        {
            weaponSlotManager.LoadWeaponSlot(RightWeapon,false);
            weaponSlotManager.LoadWeaponSlot(LeftWeapon,true);
        }
    }
}


