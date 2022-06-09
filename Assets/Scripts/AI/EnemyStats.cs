using System;
using System.Collections;
using System.Collections.Generic;
using AI.Manager;
using Player.Canvas;
using Player.Locomotion;
using Player.Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AI.Stats
{
    public class EnemyStats : CharacterStats, IDamageable
    {
        private Animator animator;
        private EnemyManager _enemyManager;
        private CanvasEnemyManager _canvasEnemyManager;

        private void Awake()
        {
            healthSystem = new HealthSystem(MaxHealth);
            _canvasEnemyManager = GetComponentInChildren<CanvasEnemyManager>();
            animator = GetComponentInChildren<Animator>();
            _enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            _canvasEnemyManager.StartHealthValue(healthSystem.GetHealthNormalized());
        }
        

        private int SetMaxHealthFromHealthLevel()
        {
            MaxHealth = HealthLevel * 10;
            return MaxHealth;
        }

        public void TakeDamage(int damageAmount, string damageAnimation = "Damage_01")
        {
            if(healthSystem.IsDead()) return;
            
            healthSystem.Damage(damageAmount);
            animator.Play("Damage_01");
            print(healthSystem.CurrentHealth);
            if(!_enemyManager.CurrentTarget) _enemyManager.transform.LookAt(PlayerManager.Instance.transform);
            _canvasEnemyManager.UpdateHealthValue(healthSystem.GetHealthNormalized());
            if(healthSystem.IsDead()) OnDead();
            
        }

        private void OnDead()
        {
            animator.Play("Dead_01");
            Destroy(_enemyManager);
        }
    }
}



