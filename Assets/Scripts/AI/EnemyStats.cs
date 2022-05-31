using System;
using System.Collections;
using System.Collections.Generic;
using AI.Manager;
using Player.Canvas;
using Player.Locomotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AI.Stats
{
    public class EnemyStats : MonoBehaviour, IDamageable
    {

        [BoxGroup("STATS")] public int HealthLevel = 10;
        [BoxGroup("STATS")] public int MaxHealth;
        public int CurrentHealth => healthSystem.CurrentHealth;

        public HealthSystem healthSystem;


        private Animator animator;
        private EnemyManager _enemyManager;

        private void Awake()
        {
            healthSystem = new HealthSystem(MaxHealth);
        }

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            _enemyManager = GetComponent<EnemyManager>();
        }
        

        private int SetMaxHealthFromHealthLevel()
        {
            MaxHealth = HealthLevel * 10;
            return MaxHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            if(healthSystem.IsDead()) return;
            
            healthSystem.Damage(damageAmount);
            animator.Play("Damage_01");
            print(healthSystem.CurrentHealth);
            
            if(healthSystem.IsDead()) OnDead();
            
        }

        private void OnDead()
        {
            animator.Play("Dead_01");
            Destroy(_enemyManager);
        }
    }
}



