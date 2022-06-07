using System;
using Inventory;
using Player.Input;
using Player.Stats;
using UnityEngine;

namespace Managers
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        private PlayerWeaponInventory _playerWeaponInventory;
        public BlockingCollider _blockingCollider;

        private void Awake()
        {
            _playerWeaponInventory = GetComponentInParent<PlayerWeaponInventory>();
        }


        public void OpenBlockingCollider()
        {
            if(!_playerWeaponInventory.LeftWeapon) return;
            _blockingCollider.SetColliderDamageAbsorption(_playerWeaponInventory.LeftWeapon);
            _blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            _blockingCollider.DisableBlockingCollider();
        }
    }
}