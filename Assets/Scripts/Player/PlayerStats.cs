using System;
using System.Collections;
using System.Collections.Generic;
using Player.Canvas;
using Player.Locomotion;
using Player.Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Stats
{
    public class PlayerStats : CharacterStats, IDamageable
    {

        [BoxGroup("Canvas")] public PlayerCanvas PlayerCanvas;
        
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerManager _playerManager;

        private void Awake()
        {
            healthSystem = new HealthSystem(MaxHealth);
            _playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            PlayerCanvas = GetComponentInChildren<PlayerCanvas>();
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();

            PlayerCanvas.SetHPSlider(healthSystem.CurrentHealth);
        }


        private void Update()
        {
            if (Keyboard.current[UnityEngine.InputSystem.Key.M].wasPressedThisFrame)
            {
                TakeDamage(10);
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            MaxHealth = HealthLevel * 10;
            return MaxHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            if(_playerManager.IsInvulnerable) return;
            if(healthSystem.IsDead()) return;
            
            healthSystem.Damage(damageAmount);
            PlayerCanvas.SetCurrentHealth(healthSystem.CurrentHealth);
            _playerAnimatorManager.PlayTargetAnimation("Damage_01", true);
            
            if(healthSystem.IsDead()) OnDead();
            
        }

        public void HealPlayer(int healAmount)
        {
            healthSystem.Heal(healAmount);
            PlayerCanvas.SetCurrentHealth(healthSystem.CurrentHealth);
        }

        private void OnDead()
        {
            _playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }
}



