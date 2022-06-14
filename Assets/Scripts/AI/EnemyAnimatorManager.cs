using System;
using Inventory.Item;
using Managers;
using MoreMountains.Tools;
using UnityEngine;

namespace AI.Manager
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager _enemyManager;
        private EnemyWeaponSlotManager _enemyWeaponSlotManager;


        private void Awake()
        {
            Animator = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
            _enemyWeaponSlotManager = GetComponent<EnemyWeaponSlotManager>();
        }

        
        public void Rotate()
        {
            Animator.SetBool("canRotate", true);
        }
        public void StopRotation()
        {
            Animator.SetBool("canRotate", false);
        }
        public void EnableCombo()
        {
            Animator.SetBool("canCombo", true);
        }
        public void DisableCombo()
        {
            Animator.SetBool("canCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            Animator.SetBool("IsInvulnerable", true);
        }

        public void DisableIsInvulnerable()
        {
            Animator.SetBool("IsInvulnerable", false);
        }
        
        public void VFX(GameObject vfx)
        {
            var _vfx = Instantiate(vfx, transform.position, _enemyManager.transform.rotation);

            if (_vfx.TryGetComponent(out MMParentingOnStart parentingOnStart))
            {
                parentingOnStart.TargetParent = _enemyManager.transform;
            }


        }
        
        public void AreaDamage(int radius)
        {
            _enemyWeaponSlotManager._rightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>().AreaDamage(radius);
        }
        
        private void OnAnimatorMove()
        {
            if(_enemyManager.IsInteracting == false) return;
            float delta = Time.deltaTime;

            Vector3 deltaPosition = Animator.deltaPosition;
            Vector3 velocity = deltaPosition / delta;
            _enemyManager.Agent.velocity = velocity;
        }
    }
}