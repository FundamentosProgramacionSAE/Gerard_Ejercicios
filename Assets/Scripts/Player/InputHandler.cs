using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Manager;
using Inventory;
using Inventory.Item;
using Player.CameraManager;
using Player.Canvas;
using Player.Locomotion;
using Player.Manager;
using Player.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Input
{
    public class InputHandler : MonoBehaviour
    {
        [TitleGroup("Values")]
        public float Horizontal;
        public float Vertical;
        public float MoveAmount;
        public float MouseX;
        public float MouseY;

        [TitleGroup("Flags")]
        public bool SprintFlag;
        public bool ComboFlag;
        public bool InventoryFlag;
        public bool LockOnFlag;

        [TitleGroup("Inputs")]
        public bool input;
        public bool BlockInput;
        public bool RollInput;
        public bool Rb_Input;
        public bool ReposeInput;
        public bool SecondAbilityInput;
        public bool ThirdAbilityInput;
        public bool FourthAbilityInput;
        public bool InventoryInput;
        public bool JumpInput;
        public bool LockOnInput;
        public bool InteractInput;
        public bool PrimarySlotInput;
        public bool SecondarySlotInput;
        
        [TitleGroup("Slots")]
        public SlotFlasks PrimarySlot;
        public SlotFlasks SecondarySlot;






        private CameraHandler _cameraHandler;
        private PlayerLocomotion _playerLocomotion;
        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerWeaponInventory _playerWeaponInventory;
        private PlayerStats playerStats;
        private PlayerManager playerManager;
        private PlayerAnimatorManager _playerAnimatorManager;
        private AbilityManager abilityManager;
        private PlayerCanvas playerCanvas;
        private WeaponSlotManager _weaponSlotManager;

        private Vector2 movementInput;
        private Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            _playerWeaponInventory = GetComponent<PlayerWeaponInventory>();
            playerStats = GetComponent<PlayerStats>();
            playerManager = GetComponent<PlayerManager>();
            abilityManager = GetComponent<AbilityManager>();
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerCanvas = GetComponentInChildren<PlayerCanvas>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        private void Start()
        {
            _cameraHandler = CameraHandler.Instance;
        }


        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();
                
                inputActions.PlayerMovement.Camera.performed +=
                    inputActions => cameraInput = inputActions.ReadValue<Vector2>();
                
                inputActions.PlayerActions.Sprint.performed += i => input = true;
                inputActions.PlayerActions.Sprint.canceled += i => input = false;
                
                inputActions.PlayerActions.Block.performed += i => BlockInput = true;
                inputActions.PlayerActions.Block.canceled += i => BlockInput = false;
                
                inputActions.PlayerActions.Jump.performed += i => JumpInput = true;
                inputActions.PlayerActions.Roll.performed += i => RollInput = true;
                
                inputActions.PlayerActions.RB.performed += i => Rb_Input = true;
                
                inputActions.PlayerActions.Ability2.performed += i => SecondAbilityInput = true;
                inputActions.PlayerActions.Ability3.performed += i => ThirdAbilityInput = true;
                inputActions.PlayerActions.Ability4.performed += i => FourthAbilityInput = true;
                
                inputActions.PlayerActions.Inventory.performed += i => InventoryInput = true;
                inputActions.PlayerActions.ReposeWeapon.performed += i => ReposeInput = true;
                
                inputActions.PlayerActions.LockOn.performed += i => LockOnInput = true;

                inputActions.PlayerActions.Interact.performed += i => InteractInput = true;
                inputActions.PlayerActions.PrimarySlot.performed += i => PrimarySlotInput = true;
                inputActions.PlayerActions.SecondarySlot.performed += i => SecondarySlotInput = true;

            }
            
            inputActions.Enable();
        }
        
        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if(playerStats.healthSystem.IsDead()) return;
            HandleInventoryInput();
            if (InventoryFlag)
            {
                Horizontal = 0;
                Vertical = 0;
                MoveAmount = 0;
                movementInput = Vector2.zero;
                return;
            }
            
            HandleSlotInputs();
            HandleJumpingInput();
            MoveInput();
            HandleRollInput();
            HandleSprintInput();
            AttackInputs();
            HandleLockOnInput();

        }

        public void AttackInputs()
        {
            HandleAttackInput();
            HandleReposeInput();
        }

        private void MoveInput()
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
            MouseX = cameraInput.x;
            MouseY = cameraInput.y;
        }
        
        private void HandleRollInput()
        {
            if (RollInput)
            {
                RollInput = false;
                _playerLocomotion.HandleRollingAndSprinting();
            }
        }

        private void HandleSprintInput()
        {
            // Cuando camina
            if (input && MoveAmount > 0.5f)
            {
                SprintFlag = true;
            }
            else
            {
                SprintFlag = false;
            }
        }

        private void HandleAttackInput()
        {
            if(_playerWeaponInventory.RightWeapon == null) return;
            
            
            if(_playerWeaponInventory.RightWeapon.IsUnarmed) return;
            
            
            
            // RB input handles the RIGHT hand weapon's light attack
            if (Rb_Input && playerManager.IsBlocking == false)
            {
                HandleUnReposeWeapon();

                SetWeaponDamageCollider();
                if (playerManager.CanCombo)
                {
                    ComboFlag = true;
                    playerAttacker.HandleWeaponCombo(_playerWeaponInventory.RightWeapon);
                    ComboFlag = false;
                }
                else
                {
                    if(playerManager.IsInteracting) return;
                    if(playerManager.CanCombo) return;
                    
                    playerAttacker.HandleLightAttack(_playerWeaponInventory.RightWeapon);
                }

            }

            if (BlockInput)
            {
                // Block
                playerAttacker.HandleBlock();
            }
            else
            {
                playerManager.IsBlocking = false;
            }

            if(playerManager.IsInteracting || abilityManager.HasAbilities != true || playerManager.IsBlocking) return;
            if (SecondAbilityInput && abilityManager.CanUseAbility2)
            {
                HandleUnReposeWeapon();
                SetWeaponDamageCollider();
                playerAttacker.HandleAbilityAttack2(_playerWeaponInventory.RightWeapon);
                abilityManager.CurrentAbility = abilityManager.Ability2;
                abilityManager.RestartCooldownAbility2();
                abilityManager.CanUseAbility2 = false;
            }
            
            if (ThirdAbilityInput && abilityManager.CanUseAbility3)
            {
                HandleUnReposeWeapon();
                SetWeaponDamageCollider();
                playerAttacker.HandleAbilityAttack3(_playerWeaponInventory.RightWeapon);
                abilityManager.CurrentAbility = abilityManager.Ability3;
                abilityManager.RestartCooldownAbility3();
                abilityManager.CanUseAbility3 = false;
            }
            if (FourthAbilityInput && abilityManager.CanUseAbility4)
            {
                HandleUnReposeWeapon();
                SetWeaponDamageCollider();
                playerAttacker.HandleAbilityAttack4(_playerWeaponInventory.RightWeapon);
                abilityManager.CurrentAbility = abilityManager.Ability4;
                abilityManager.RestartCooldownAbility4();
                abilityManager.CanUseAbility4 = false;
            }
        }
        
        private void SetWeaponDamageCollider()
        {
            if (_playerWeaponInventory.RightWeapon.IsDualWeapon)
            {
                _playerAnimatorManager.Animator.SetBool("IsUsingRightHand", true);
                _playerAnimatorManager.Animator.SetBool("IsUsingLeftHand", true);
            }
            else
            {
                _playerAnimatorManager.Animator.SetBool("IsUsingRightHand", true);
            }
        }

        private void HandleReposeInput()
        {
            
            if(_playerWeaponInventory.RightWeapon == null) return;
            
            if(_playerWeaponInventory.RightWeapon.IsUnarmed || LockOnFlag) return;
            
            if (ReposeInput)
            {
                if (playerManager.IsReposeWeapon)
                {
                    _playerAnimatorManager.Animator.SetBool("isReposeWeapon", false);
                    _playerAnimatorManager.PlayTargetAnimation("Equip", false);
                    playerManager.IsReposeWeapon = false;
                }
                else
                {
                    _playerAnimatorManager.Animator.SetBool("isReposeWeapon", true);
                    _playerAnimatorManager.PlayTargetAnimation("Unequip", false);
                    playerManager.IsReposeWeapon = true;
                }
            }
        }

        private void HandleInventoryInput()
        {
            if (InventoryInput)
            {
                InventoryFlag = !InventoryFlag;
                
                if (InventoryFlag)
                {
                    playerCanvas.OpenInventory();
                }
                else
                {
                    playerCanvas.CloseInventory();
                }
            }
        }

        private void HandleJumpingInput()
        {
            if (JumpInput)
            {
                JumpInput = false;
                _playerLocomotion.HandleJumping();
            }
        }

        private void HandleUnReposeWeapon()
        {
            if(playerManager.IsReposeWeapon == false) return; 
            if (playerManager.IsReposeWeapon)
            {
                _weaponSlotManager.OnEquipWeapon();
                playerManager.IsReposeWeapon = false;
                _playerAnimatorManager.Animator.SetBool("isReposeWeapon", false);
                    
            }
        }

        private void HandleLockOnInput()
        {
            if (LockOnInput && LockOnFlag == false)
            {
                _cameraHandler.ClearLockOnTargets();
                LockOnInput = false;
                LockOnFlag = false;
                _cameraHandler.HandleLockOn();
                if (_cameraHandler._nearestLockOn != null)
                {
                    _cameraHandler._currentLockOnTarget = _cameraHandler._nearestLockOn;
                    _cameraHandler.VirtualCamera.LookAt = _cameraHandler._currentLockOnTarget.LockOn;
                    if (playerManager.IsReposeWeapon)
                    {                    _playerAnimatorManager.Animator.SetBool("isReposeWeapon", false);
                        _playerAnimatorManager.PlayTargetAnimation("Equip", false);
                        playerManager.IsReposeWeapon = false;
                        
                    }

                    LockOnFlag = true;
                }
            }
            else if (LockOnInput && LockOnFlag)
            {
                ClearCamera();
            }
            
        }

        private void HandleSlotInputs()
        {
            if (PrimarySlotInput && PrimarySlot.SlotItem)
            {
                if (PrimarySlot.SlotItem.IsUsingItem == false)
                {
                    PrimarySlot.SlotItem.ListPotions(PrimarySlot.SlotItem.ItemData as FlaskItem);
                    EventSystem.Instance.OnHealPlayer(PrimarySlot.SlotItem.ItemData as FlaskItem);
                }

            }
            if (SecondarySlotInput && SecondarySlot.SlotItem)
            {
                if (SecondarySlot.SlotItem.IsUsingItem == false)
                {
                    SecondarySlot.SlotItem.ListPotions(SecondarySlot.SlotItem.ItemData as FlaskItem);
                    EventSystem.Instance.OnHealPlayer(SecondarySlot.SlotItem.ItemData as FlaskItem);
                }
            }
        }

        public void ClearCamera()
        {
            LockOnInput = false;
            LockOnFlag = false;
            _cameraHandler.ClearLockOnTargets();
        }
    }
}


