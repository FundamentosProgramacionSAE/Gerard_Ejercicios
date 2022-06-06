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
        public static PlayerManager Instance { get; private set; }
        
        [TitleGroup("Inputs")]
        public bool IsInteracting;
        public bool IsSprinting;
        public bool IsInAir;
        public bool IsGrounded;
        public bool IsJumping;
        public bool CanCombo;
        public bool IsReposeWeapon;
        
        [TitleGroup("Flags")]
        public bool IsUsingRightHand;
        public bool IsUsingLeftHand;
        public bool IsInvulnerable;
        
        private InputHandler inputHandler;
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerLocomotion playerLocomotion;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();

        }

        private void Update()
        {
            IsInteracting = _playerAnimatorManager.Animator.GetBool("isInteracting");
            CanCombo = _playerAnimatorManager.Animator.GetBool("canCombo");
            IsUsingRightHand = _playerAnimatorManager.Animator.GetBool("IsUsingRightHand");
            IsUsingLeftHand = _playerAnimatorManager.Animator.GetBool("IsUsingLeftHand");
            IsInvulnerable = _playerAnimatorManager.Animator.GetBool("IsInvulnerable");

            inputHandler.TickInput();


        }

        private void FixedUpdate()
        {
            playerLocomotion.Handles();
            //_cameraHandler.HandleCameraMovement();

        }

        private void LateUpdate()
        {

            inputHandler.Rb_Input = false;
            inputHandler.SecondAbilityInput = false;
            inputHandler.ThirdAbilityInput = false;
            inputHandler.FourthAbilityInput = false;
            inputHandler.ReposeInput = false;
            inputHandler.InventoryInput = false;
            inputHandler.JumpInput = false;
            IsJumping = _playerAnimatorManager.Animator.GetBool("isJumping");
            _playerAnimatorManager.Animator.SetBool("isGrounded", IsGrounded);
        }
    }
}


