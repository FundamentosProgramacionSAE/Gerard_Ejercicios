using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player.Input;
using Player.Locomotion;
using Player.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Manager
{
    public class PlayerManager : MonoBehaviour
    {

        [BoxGroup("Inputs")] public bool IsInteracting;
        [BoxGroup("Inputs")] public bool IsSprinting;
        [BoxGroup("Inputs")] public bool IsInAir;
        [BoxGroup("Inputs")] public bool IsGrounded;
        [BoxGroup("Inputs")] public bool IsJumping;
        [BoxGroup("Inputs")] public bool CanCombo;
        [BoxGroup("Inputs")] public bool IsReposeWeapon;

        public CinemachineBrain CinemachineBrain;
        
        private InputHandler inputHandler;
        private AnimatorHandler animatorHandler;
        private PlayerLocomotion playerLocomotion;
        


        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Update()
        {
            IsInteracting = animatorHandler.Animator.GetBool("isInteracting");
            CanCombo = animatorHandler.Animator.GetBool("canCombo");
            inputHandler.TickInput();

        }

        private void FixedUpdate()
        {
            playerLocomotion.Handles();
        }

        private void LateUpdate()
        {
            inputHandler.RollFlag = false;

            inputHandler.SecondAbilityInput = false;
            inputHandler.ThirdAbilityInput = false;
            inputHandler.FourthAbilityInput = false;
            inputHandler.ReposeInput = false;
            inputHandler.InventoryInput = false;
            inputHandler.JumpInput = false;
            IsJumping = animatorHandler.Animator.GetBool("isJumping");
            animatorHandler.Animator.SetBool("isGrounded", IsGrounded);
        }
    }
}


