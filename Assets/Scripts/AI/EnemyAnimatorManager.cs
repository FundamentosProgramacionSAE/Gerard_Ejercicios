using System;
using Managers;
using UnityEngine;

namespace AI.Manager
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager _enemyManager;


        private void Awake()
        {
            Animator = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
        }

        
        public void Rotate()
        {
            Animator.SetBool("canRotate", true);
        }
        public void StopRotation()
        {
            Animator.SetBool("canRotate", false);
        }
        public void EnableCombo()
        {
            Animator.SetBool("canCombo", true);
        }
        public void DisableCombo()
        {
            Animator.SetBool("canCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            Animator.SetBool("IsInvulnerable", true);
        }

        public void DisableIsInvulnerable()
        {
            Animator.SetBool("IsInvulnerable", false);
        }
        
        private void OnAnimatorMove()
        {
            if(_enemyManager.IsInteracting == false) return;
            float delta = Time.deltaTime;
            //_enemyManager._enemyRigidbody.drag = 0;
            Vector3 deltaPosition = Animator.deltaPosition;
            Vector3 velocity = deltaPosition / delta;
            _enemyManager.Agent.velocity = velocity;
        }
    }
}