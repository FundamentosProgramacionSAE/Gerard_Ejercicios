using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Manager;
using Inventory;
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
        [BoxGroup("Values")] public float Horizontal;
        [BoxGroup("Values")] public float Vertical;
        [BoxGroup("Values")] public float MoveAmount;
        [BoxGroup("Values")] public float RollInputTimer;
        

        [BoxGroup("Flags")] public bool RollFlag;
        [BoxGroup("Flags")] public bool SprintFlag;
        [BoxGroup("Flags")] public bool ComboFlag;
        [BoxGroup("Flags")] public bool InventoryFlag;

        [BoxGroup("Inputs")] public bool input;
        [BoxGroup("Inputs")] public bool Rb_Input;
        [BoxGroup("Inputs")] public bool ReposeInput;
        [BoxGroup("Inputs")] public bool SecondAbilityInput;
        [BoxGroup("Inputs")] public bool ThirdAbilityInput;
        [BoxGroup("Inputs")] public bool FourthAbilityInput;
        [BoxGroup("Inputs")] public bool InventoryInput;
        



        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerWeaponInventory _playerWeaponInventory;
        private PlayerStats playerStats;
        private PlayerManager playerManager;
        private AnimatorHandler animatorHandler;
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
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerCanvas = GetComponentInChildren<PlayerCanvas>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();
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

            MoveInput();
            HandleRollInput();
            HandleSprintInput();
            AttackInputs();

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
        }
        

        private void HandleRollInput()
        {
            input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            
            if (input && MoveAmount >= 1)
            {
                RollFlag = true;
            }
        }

        private void HandleSprintInput()
        {
            input = inputActions.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            
            if (input)
            {
                SprintFlag = true;
            }
        }

        private void HandleAttackInput()
        {
            if(_playerWeaponInventory.RightWeapon.IsUnarmed) return;
            
            inputActions.PlayerActions.RB.performed += i => Rb_Input = true;
            inputActions.PlayerActions.Ability2.performed += i => SecondAbilityInput = true;
            inputActions.PlayerActions.Ability3.performed += i => ThirdAbilityInput = true;
            inputActions.PlayerActions.Ability4.performed += i => FourthAbilityInput = true;

            // RB input handles the RIGHT hand weapon's light attack
            if (Rb_Input)
            {
                UnReposeWeapon();
                
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
            
            if(playerManager.IsInteracting || abilityManager.HasAbilities != true) return;
            if (SecondAbilityInput && abilityManager.CanUseAbility2)
            {
                UnReposeWeapon();
                playerAttacker.HandleAbilityAttack2(_playerWeaponInventory.RightWeapon);
                abilityManager.RestartCooldownAbility2();
                abilityManager.CanUseAbility2 = false;
            }
            
            if (ThirdAbilityInput && abilityManager.CanUseAbility3)
            {
                UnReposeWeapon();
                playerAttacker.HandleAbilityAttack3(_playerWeaponInventory.RightWeapon);
                abilityManager.RestartCooldownAbility3();
                abilityManager.CanUseAbility3 = false;
            }
            if (FourthAbilityInput && abilityManager.CanUseAbility4)
            {
                UnReposeWeapon();
                playerAttacker.HandleAbilityAttack4(_playerWeaponInventory.RightWeapon);
                abilityManager.RestartCooldownAbility4();
                abilityManager.CanUseAbility4 = false;
            }
        }
        private void HandleReposeInput()
        {
            if(_playerWeaponInventory.RightWeapon.IsUnarmed) return;
            inputActions.PlayerActions.ReposeWeapon.performed += i => ReposeInput = true;

            if (ReposeInput)
            {
                if (playerManager.IsReposeWeapon)
                {
                    animatorHandler.Animator.SetBool("isReposeWeapon", false);
                    animatorHandler.PlayTargetAnimation("Equip", false);
                    playerManager.IsReposeWeapon = false;
                }
                else
                {
                    animatorHandler.Animator.SetBool("isReposeWeapon", true);
                    animatorHandler.PlayTargetAnimation("Unequip", false);
                    playerManager.IsReposeWeapon = true;
                }
            }
        }

        private void HandleInventoryInput()
        {
            inputActions.PlayerActions.Inventory.performed += i => InventoryInput = true;

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

        private void UnReposeWeapon()
        {
            if (playerManager.IsReposeWeapon)
            {
                _weaponSlotManager.OnEquipWeapon();
                playerManager.IsReposeWeapon = false;
                animatorHandler.Animator.SetBool("isReposeWeapon", false);
                    
            }
        }
    }
}


