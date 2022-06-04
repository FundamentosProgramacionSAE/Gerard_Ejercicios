using System;
using Player.Manager;
using UnityEngine;
using  UnityEngine.AI;


namespace AI.Manager
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        public EnemyManager _enemyManager;

        private EnemyAnimatorManager _enemyAnimatorManager;

        public CapsuleCollider CharacterCollider;
        public CapsuleCollider CharacterBlockerCollider;
        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Start()
        {
            Physics.IgnoreCollision(CharacterCollider, CharacterBlockerCollider, true);
        }


        public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}