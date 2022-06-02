using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI.States
{
    public class ChaseState : State
    {
        
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.CHASE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTER CHASE STATE");

            }
        
            return EnteredState;
        }
        
        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position,
                enemyManager.transform.position);
            Vector3 targetDirection = enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            
            
            if (enemyManager.IsPreformingAction)
            {
                enemyAnimatorManager.Animator.SetFloat("Vertical", 0, 0.1f,Time.deltaTime);
                return;
            }
            
            HandleMoveToTarget(enemyManager,enemyAnimatorManager);

            if (distanceFromTarget <= enemyManager.MaxAttackRange)
            {
                enemyManager.EnterState(FSMStateType.COMBAT);
            }
            //Chase state
            // If is in range of attack, switch to combat state
            // if target ir out of range, return this state and continue to chase target
        }
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING CHASE STATE");
            return true;
        }
        
        
        public void HandleMoveToTarget(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            
            if (distanceFromTarget > enemyManager.MaxAttackRange)
            {
                enemyAnimatorManager.Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager,distanceFromTarget);
            Agent.transform.localPosition = Vector3.zero;
            Agent.transform.localRotation = Quaternion.identity;
            
        }
        
        public void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
        {
            if (enemyManager.IsPreformingAction)
            {
                Vector3 direction = enemyManager.CurrentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }
                
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, EnemyManager.RotationSpeed / Time.deltaTime);

            }
            // else
            // {
            //     Vector3 targetVelocity = enemyManager.EnemyLocomotion._enemyRigidbody.velocity;
            //
            //     Agent.enabled = true;
            //     Agent.SetDestination(EnemyManager.CurrentTarget.transform.position);
            //     enemyManager.EnemyLocomotion._enemyRigidbody.velocity = targetVelocity;
            //     enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.EnemyLocomotion.Agent.transform.rotation, enemyManager.RotationSpeed / Time.deltaTime);
            // }
            else
            {

                enemyManager.EnemyLocomotion.Agent.enabled = true;
                enemyManager.EnemyLocomotion.Agent.SetDestination(enemyManager.CurrentTarget.transform.position);

                float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation,
                    Quaternion.LookRotation(enemyManager.EnemyLocomotion.Agent.desiredVelocity.normalized));
                if (distanceFromTarget > 5) enemyManager.EnemyLocomotion.Agent.angularSpeed = 500f;
                else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30)
                    enemyManager.EnemyLocomotion.Agent.angularSpeed = 50f;
                else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30)
                    enemyManager.EnemyLocomotion.Agent.angularSpeed = 500f;

                Vector3 targetDirection =
                    enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);


                if (enemyManager.EnemyLocomotion.Agent.desiredVelocity.magnitude > 0)
                {
                    enemyManager.EnemyLocomotion.Agent.updateRotation = false;
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                        Quaternion.LookRotation(enemyManager.EnemyLocomotion.Agent.desiredVelocity.normalized),
                        enemyManager.EnemyLocomotion.Agent.angularSpeed * Time.deltaTime);
                }
                else
                {
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                        rotationToApplyToStaticEnemy, enemyManager.EnemyLocomotion.Agent.angularSpeed * Time.deltaTime);
                }
            }

        }
    }
}