using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using InteractableItems;
using Player.CameraManager;
using Player.Input;
using Player.Locomotion;
using Player.Stats;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private InteractableUI _interactableUI;


        private void Awake()
        {
            Instance = this;
            
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            _interactableUI = GetComponentInChildren<InteractableUI>();
        }
        

        private void Update()
        {
            IsInteracting = _playerAnimatorManager.Animator.GetBool("isInteracting");
            CanCombo = _playerAnimatorManager.Animator.GetBool("canCombo");
            IsUsingRightHand = _playerAnimatorManager.Animator.GetBool("IsUsingRightHand");
            IsUsingLeftHand = _playerAnimatorManager.Animator.GetBool("IsUsingLeftHand");
            IsInvulnerable = _playerAnimatorManager.Animator.GetBool("IsInvulnerable");
            _playerAnimatorManager.CanRotate = _playerAnimatorManager.Animator.GetBool("canRotate");
            _playerAnimatorManager.Animator.SetBool("isBlocking", IsBlocking);
            
            inputHandler.TickInput();
            CheckInteractable();
        }

        private void FixedUpdate()
        {
            playerLocomotion.Handles();
            playerLocomotion.HandleRotation(Time.fixedDeltaTime);
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
            inputHandler.InteractInput = false;
            inputHandler.PrimarySlotInput = false;
            inputHandler.SecondarySlotInput = false;
            IsJumping = _playerAnimatorManager.Animator.GetBool("isJumping");
            _playerAnimatorManager.Animator.SetBool("isGrounded", IsGrounded);
        }


        private void CheckInteractable()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    var interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null && interactable.IsCollected == false)
                    {
                        string interactableText = interactable.InteractableText;
                        _interactableUI.InteractableText.text = interactableText;
                        _interactableUI.InteractableGameObject.SetActive(true);

                        if (inputHandler.InteractInput)
                        {
                           interactable.Interact(this);
                           interactable.IsCollected = true;
                           _interactableUI.InteractableGameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                if (_interactableUI.InteractableGameObject != null)
                {
                    _interactableUI.InteractableGameObject.SetActive(false);
                }
            }

        }

        public void OpenChestInteraction(Transform playerStand, string nameItem, Sprite sprite)
        {
            playerLocomotion.Rigidbody.velocity = Vector3.zero;
            transform.position = playerStand.position;
            _playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
            _interactableUI.InteractableObjectPicked.SetActive(true);
            _interactableUI.InteractableObjectPicked.GetComponentInChildren<TextMeshProUGUI>().text = nameItem;
            _interactableUI.ImageItem.sprite = sprite;
            _interactableUI.PlayFeedbackOnCollected();;

        }
    }
}


