using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;

namespace Inventory
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform ParentOverride;
        public bool IsLeftHandSlot;
        public bool IsRightHandSlot;
        public GameObject CurrentWeaponModel;
        public bool isRest;


        public void UnloadWeapon()
        {
            if (CurrentWeaponModel != null)
            {
                CurrentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (CurrentWeaponModel != null)
            {
                Destroy(CurrentWeaponModel);
            }
        }


        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();
            
            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }


            GameObject model = Instantiate(weaponItem.ModelPrefab);

            if (model.GetComponentInChildren<DamageCollider>())
            {
                model.GetComponentInChildren<DamageCollider>().WeaponDamage = weaponItem.WeaponDamage;
            }


            if (model != null)
            {
                if (ParentOverride != null)
                {
                    model.transform.parent = ParentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }
                
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            CurrentWeaponModel = model;
        }

    }
}


