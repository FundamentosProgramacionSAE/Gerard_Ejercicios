using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI.States
{
    public class CombatState : State
    {
        
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.COMBAT;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTER COMBAT STATE");
                //EnemyManager.transform.LookAt(EnemyManager.CurrentTarget.transform);

            }
        
            return EnteredState;
        }
        
        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //Check for attack range
            // potential circle player or walk around them
            // if in attack range return attack state
            // if we are in cooldown after attack, return this state and continue circling target
            // if the player runs out of range return to chase state
            if(enemyManager.IsInteracting) return;
            
            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position,
                enemyManager.transform.position);
            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);

            if (enemyManager.IsPreformingAction)
            {
                enemyAnimatorManager.Animator.SetFloat("Vertical", 0,0.1f, Time.deltaTime);
            }

            if (enemyManager.CurrentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.MaxAttackRange)
            {
                enemyManager.EnterState(FSMStateType.ATTACK);
            }
            else if (distanceFromTarget > enemyManager.MaxAttackRange)
            {
                enemyManager.EnterState(FSMStateType.CHASE);
            }

        }
        
        private void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
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
            else
            {

                enemyManager.Agent.enabled = true;
                enemyManager.Agent.SetDestination(enemyManager.CurrentTarget.transform.position);

                float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation,
                    Quaternion.LookRotation(enemyManager.Agent.desiredVelocity.normalized));
                if (distanceFromTarget > 5) enemyManager.Agent.angularSpeed = 500f;
                else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30)
                    enemyManager.Agent.angularSpeed = 50f;
                else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30)
                    enemyManager.Agent.angularSpeed = 500f;

                Vector3 targetDirection =
                    enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);


                if (enemyManager.Agent.desiredVelocity.magnitude > 0)
                {
                    enemyManager.Agent.updateRotation = false;
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                        Quaternion.LookRotation(enemyManager.Agent.desiredVelocity.normalized),
                        enemyManager.Agent.angularSpeed * Time.deltaTime);
                }
                else
                {
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                        rotationToApplyToStaticEnemy, enemyManager.Agent.angularSpeed * Time.deltaTime);
                }
            }

        }
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING COMBAT STATE");
            return true;
        }
    }
}