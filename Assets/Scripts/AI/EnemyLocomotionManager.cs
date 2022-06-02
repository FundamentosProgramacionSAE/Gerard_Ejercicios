using System;
using Player.Manager;
using UnityEngine;
using  UnityEngine.AI;


namespace AI.Manager
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        public Rigidbody _enemyRigidbody;
        public EnemyManager _enemyManager;
        public NavMeshAgent Agent;
        

        private EnemyAnimatorManager _enemyAnimatorManager;
        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            Agent = GetComponentInChildren<NavMeshAgent>();
            _enemyRigidbody = GetComponent<Rigidbody>();
        }


        private void Start()
        {
            Agent.enabled = false;
            _enemyRigidbody.isKinematic = false;
        }
        
        

        public void HandleMoveToPosition(Vector3 CurrentTarget, float valueSpeed)
        {
            if(_enemyManager.IsPreformingAction) return;

            Vector3 targetDirection = CurrentTarget - transform.position;
            float distanceFromTarget = Vector3.Distance(CurrentTarget, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            
            if (distanceFromTarget > _enemyManager.StoppingDistance)
            {
                _enemyAnimatorManager.Animator.SetFloat("Vertical", valueSpeed, 0.1f, Time.deltaTime);
            }
            else if(distanceFromTarget <= _enemyManager.StoppingDistance)
            {
                _enemyAnimatorManager.Animator.SetFloat("Vertical",0,0.1f,Time.deltaTime);
            }

            HandleRotateTowardsTarget(CurrentTarget);
            
            Agent.transform.localPosition = Vector3.zero;
            Agent.transform.localRotation = Quaternion.identity;
            
        }

        public void HandleRotateTowardsTarget(Vector3 CurrentTarget)
        {
            if (_enemyManager.IsPreformingAction)
            {
                Vector3 direction = CurrentTarget - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, _enemyManager.RotationSpeed / Time.deltaTime);

            }
            else
            {
                Vector3 realitveDirection = transform.InverseTransformDirection(Agent.desiredVelocity);
                Vector3 targetVelocity = _enemyRigidbody.velocity;

                Agent.enabled = true;
                Agent.SetDestination(CurrentTarget);
                _enemyRigidbody.velocity = targetVelocity;
                _enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, Agent.transform.rotation, _enemyManager.RotationSpeed / Time.deltaTime);
            }

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