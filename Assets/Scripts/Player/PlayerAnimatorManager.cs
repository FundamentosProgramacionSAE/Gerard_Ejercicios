using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player.Input;
using Player.Manager;
using UnityEngine;

namespace Player.Locomotion
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        public bool CanRotate;


        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private PlayerManager playerManager;
        private PlayerAttacker playerAttacker;
        
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
            CanRotate = true;
        }
        public void StopRotation()
        {
            CanRotate = false;
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


