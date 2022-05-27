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
        [BoxGroup("Inputs")] public bool UnarmedInput;
        [BoxGroup("Inputs")] public bool SecondAbilityInput;
        [BoxGroup("Inputs")] public bool ThirdAbilityInput;
        [BoxGroup("Inputs")] public bool FourthAbilityInput;
        [BoxGroup("Inputs")] public bool InventoryInput;
        



        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerStats playerStats;
        private PlayerManager playerManager;
        private AnimatorHandler animatorHandler;
        private AbilityManager abilityManager;
        private PlayerCanvas playerCanvas;

        private Vector2 movementInput;
        private Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerStats = GetComponent<PlayerStats>();
            playerManager = GetComponent<PlayerManager>();
            abilityManager = GetComponent<AbilityManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerCanvas = GetComponentInChildren<PlayerCanvas>();
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

            MoveInput();
            HandleRollInput();
            HandleSprintInput();
            AttackInputs();
            HandleInventoryInput();
        }

        public void AttackInputs()
        {
            HandleAttackInput();
            HandleUnarmedInput();
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

            if(playerManager.IsUnarmed) return;
            
            
            inputActions.PlayerActions.RB.performed += i => Rb_Input = true;
            inputActions.PlayerActions.Ability2.performed += i => SecondAbilityInput = true;
            inputActions.PlayerActions.Ability3.performed += i => ThirdAbilityInput = true;
            inputActions.PlayerActions.Ability4.performed += i => FourthAbilityInput = true;

            // RB input handles the RIGHT hand weapon's light attack
            if (Rb_Input)
            {
                if (playerManager.CanCombo)
                {
                    ComboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.RightWeapon);
                    ComboFlag = false;
                }
                else
                {
                    if(playerManager.IsInteracting) return;
                    if(playerManager.CanCombo) return;
                    
                    playerAttacker.HandleLightAttack(playerInventory.RightWeapon);
                }

            }
            
            if(playerManager.IsInteracting) return;
            if (SecondAbilityInput && abilityManager.CanUseAbility2)
            {
                playerAttacker.HandleAbilityAttack2(playerInventory.RightWeapon);
                abilityManager.RestartCooldownAbility2();
                abilityManager.CanUseAbility2 = false;
            }
            
            if (ThirdAbilityInput && abilityManager.CanUseAbility3)
            {
                playerAttacker.HandleAbilityAttack3(playerInventory.RightWeapon);
                abilityManager.RestartCooldownAbility3();
                abilityManager.CanUseAbility3 = false;
            }
            if (FourthAbilityInput && abilityManager.CanUseAbility4)
            {
                playerAttacker.HandleAbilityAttack4(playerInventory.RightWeapon);
                abilityManager.RestartCooldownAbility4();
                abilityManager.CanUseAbility4 = false;
            }
        }
        private void HandleUnarmedInput()
        {
            inputActions.PlayerActions.Unarmed.performed += i => UnarmedInput = true;

            if (UnarmedInput)
            {
                if (playerManager.IsUnarmed)
                {
                    animatorHandler.Animator.SetBool("isUnarmed", false);
                    animatorHandler.PlayTargetAnimation("Equip", false);
                    playerManager.IsUnarmed = false;
                }
                else
                {
                    animatorHandler.Animator.SetBool("isUnarmed", true);
                    animatorHandler.PlayTargetAnimation("Unequip", false);
                    playerManager.IsUnarmed = true;
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
    }
}


