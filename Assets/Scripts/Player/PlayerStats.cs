using System;
using System.Collections;
using System.Collections.Generic;
using Player.Canvas;
using Player.Locomotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Stats
{
    public class PlayerStats : MonoBehaviour, IDamageable
    {

        [BoxGroup("STATS")] public int HealthLevel = 10;
        [BoxGroup("STATS")] public int MaxHealth;

        [BoxGroup("Canvas")] public PlayerCanvas PlayerCanvas;
        
        public HealthSystem healthSystem;


        private AnimatorHandler animatorHandler;

        private void Awake()
        {
            healthSystem = new HealthSystem(MaxHealth);
        }

        private void Start()
        {
            PlayerCanvas = GetComponentInChildren<PlayerCanvas>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();

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
            healthSystem.Damage(damageAmount);
            PlayerCanvas.SetCurrentHealth(healthSystem.CurrentHealth);
            animatorHandler.PlayTargetAnimation("Damage_01", true);
            
            if(healthSystem.IsDead()) OnDead();
            
        }

        private void OnDead()
        {
            animatorHandler.PlayTargetAnimation("Dead_01", true);
        }
    }
}



