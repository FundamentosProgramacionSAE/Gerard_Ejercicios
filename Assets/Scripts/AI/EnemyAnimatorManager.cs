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

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyManager._enemyRigidbody.drag = 0;
            Vector3 deltaPosition = Animator.deltaPosition;
            Vector3 velocity = deltaPosition / delta;
            _enemyManager._enemyRigidbody.velocity = velocity;
        }
    }
}