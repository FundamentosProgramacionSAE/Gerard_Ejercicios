using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using Managers;
using MoreMountains.Tools;
using Player.Input;
using Player.Manager;
using UnityEngine;

namespace Player.Locomotion
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        public CinemachineFreeLookShake FreeLookShake;
        public CinemachineVirtualCameraShake VirtualCameraShake;
        
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private PlayerManager playerManager;
        private PlayerAttacker playerAttacker;
        private WeaponSlotManager _slot;
        
        private int vertical;
        private int horizontal;
        
        
        private bool canCombo;
        private float timer;


        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerAttacker = GetComponentInParent<PlayerAttacker>();
            _slot = GetComponentInParent<WeaponSlotManager>();
            Animator = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }
        private void Update()
        {
            canCombo = Animator.GetBool("canCombo");
            if (canCombo)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    DisableCombo();
                }
            }
        }
        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            var v = Extensions.ClampMovementDir(verticalMovement);
            var h = Extensions.ClampMovementDir(horizontalMovement);

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            
            Animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            Animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
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
            timer = playerAttacker.ComboDuration;
        }
        public void DisableCombo()
        {
            Animator.SetBool("canCombo", false);
            playerAttacker.ResetCombo();
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
            var _vfx = Instantiate(vfx, transform.position, playerManager.transform.rotation);

            if (_vfx.TryGetComponent(out MMParentingOnStart parentingOnStart))
            {
                parentingOnStart.TargetParent = playerManager.transform;
            }


        }
        
        public void EnableAreaDamage()
        {
            Animator.SetBool("IsAreaDamage", true);
        }
        public void AreaDamage(int radius)
        {
            _slot.RightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>().AreaDamage(radius);
        }
        
        public void ShakeAnimation(AnimationEvent animationEvent)
        {
            FreeLookShake.ShakeCamera(animationEvent.intParameter,animationEvent.floatParameter);
            VirtualCameraShake.ShakeCamera(animationEvent.intParameter,animationEvent.floatParameter);
        }
        private void OnAnimatorMove()
        {
            if(playerManager.IsInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocomotion.Rigidbody.drag = 0;
            Vector3 deltaPosition = Animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.Rigidbody.velocity = velocity;

        }
    }
}


