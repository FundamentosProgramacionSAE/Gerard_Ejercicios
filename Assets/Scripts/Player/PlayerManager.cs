using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player.CameraManager;
using Player.Input;
using Player.Locomotion;
using Player.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Manager
{
    public class PlayerManager : CharacterManager
    {

        [BoxGroup("Inputs")] public bool IsInteracting;
        [BoxGroup("Inputs")] public bool IsSprinting;
        [BoxGroup("Inputs")] public bool IsInAir;
        [BoxGroup("Inputs")] public bool IsGrounded;
        [BoxGroup("Inputs")] public bool IsJumping;
        [BoxGroup("Inputs")] public bool CanCombo;
        [BoxGroup("Inputs")] public bool IsReposeWeapon;


        private CameraHandler _cameraHandler;
        private InputHandler inputHandler;
        private AnimatorHandler animatorHandler;
        private PlayerLocomotion playerLocomotion;
        


        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _cameraHandler = CameraHandler.Instance;
            
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
            _cameraHandler.HandleCameraMovement();
            
            inputHandler.Rb_Input = false;
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


