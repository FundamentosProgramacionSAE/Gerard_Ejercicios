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
        
        [BoxGroup("Inputs")] public bool IsInteracting;
        [BoxGroup("Inputs")] public bool IsSprinting;
        [BoxGroup("Inputs")] public bool IsInAir;
        [BoxGroup("Inputs")] public bool IsGrounded;
        [BoxGroup("Inputs")] public bool IsJumping;
        [BoxGroup("Inputs")] public bool CanCombo;
        [BoxGroup("Inputs")] public bool IsReposeWeapon;


        [BoxGroup("Flags")] public bool IsUsingRightHand;
        [BoxGroup("Flags")] public bool IsUsingLeftHand;
        
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


