using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using Player.Locomotion;
using Player.Manager;
using Player.Stats;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Instance { get; private set; }
    public GameObject Player;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Player = PlayerManager.Instance.gameObject;
    }

    public void OnCollectWeapon()
    {
        Player.GetComponent<PlayerWeaponInventory>().Initialized();
    }

    public void OnUseWeapon(WeaponItem rightWeapon, WeaponItem leftWeapon, bool isDual, bool isShield = false)
    {
        var weaponInventory = Player.GetComponent<PlayerWeaponInventory>();
        
        if (isDual)
        {
            if (rightWeapon != weaponInventory.RightWeapon && weaponInventory.RightWeapon != null)
            {
                weaponInventory.RightWeapon.IsUsed = false;
            }
        }
        else
        {
            if (isShield == false)
            {
                if (rightWeapon != weaponInventory.RightWeapon && weaponInventory.RightWeapon != null)
                {
                    weaponInventory.RightWeapon.IsUsed = false;
                }
                if (leftWeapon != weaponInventory.LeftWeapon && weaponInventory.LeftWeapon != null)
                {
                    weaponInventory.LeftWeapon.IsUsed = false;
                }
            }
            else
            {
                if (leftWeapon != weaponInventory.LeftWeapon && weaponInventory.LeftWeapon != null)
                {
                    weaponInventory.LeftWeapon.IsUsed = false;
                }
            }

        }
        OnCollectWeapon();
    }

    public void OnUnEquipWeapon(bool isShield = false)
    {
        if (isShield == false)
        {
            Player.GetComponent<PlayerWeaponInventory>().UnEquip();
        }
        else
        {
            Player.GetComponent<PlayerWeaponInventory>().UnEquipShield();
        }

    }

    public void OnHealPlayer(FlaskItem flaskItem)
    {
        if(!flaskItem) return;
        Player.GetComponent<PlayerStats>().HealPlayer(flaskItem.HealthRecoverAmount.GetValueFromRatio());
        Player.GetComponentInChildren<InventorySystem>().Remove(flaskItem);
        Instantiate(flaskItem.RecoverFX, Player.transform.position, Quaternion.identity, Player.transform);
    }
}

