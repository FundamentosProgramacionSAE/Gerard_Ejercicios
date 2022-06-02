using System;
using Managers;
using UnityEngine;

namespace AI.Manager
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyLocomotionManager _enemyLocomotionManager;
        private void Awake()
        {
            Animator = GetComponent<Animator>();
            _enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyLocomotionManager._enemyRigidbody.drag = 0;
            Vector3 deltaPosition = Animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _enemyLocomotionManager._enemyRigidbody.velocity = velocity;
        }
    }
}