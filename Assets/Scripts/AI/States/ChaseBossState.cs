using AI.Manager;
using AI.Stats;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace AI.States
{
    public class ChaseBossState : State
    {
        public MMF_Player OnEnterWithBoss;
        private bool _firstTime = true;
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
                Agent.ResetPath();
                Agent.speed = EnemyManager.RunSpeed;
                if (EnemyManager.IsBoss)
                {
                    EnemyManager.EnemyStats.SetBossCanvas(true);

                    if (_firstTime)
                    {
                        EnemyManager._enemyAnimatorManager.PlayTargetAnimation("Power Up", true);
                        OnEnterWithBoss.PlayFeedbacks();
                        _firstTime = false;
                    }
                    
                }


            }
        
            return EnteredState;
        }
        
        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            
            if(enemyManager.IsInteracting) return;
            
            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position,
                enemyManager.transform.position);


            if (enemyManager.IsPreformingAction)
            {
                enemyManager.StopEnemy();
                return;
            }
            
            //Mueve la IA al target
            HandleMoveToTarget(enemyManager,enemyAnimatorManager);

            //Si la distancia es inferior al ataque maximo de rango, podra pasar al estado combate
            if (distanceFromTarget <= enemyManager.RangeAgro)
            {
                enemyManager.EnterState(FSMStateType.COMBAT);
            }

        }
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING CHASE STATE");
            return true;
        }
        
        
        private void HandleMoveToTarget(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            
            if (distanceFromTarget > enemyManager.MaxAttackRange)
            {
                enemyAnimatorManager.Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager,distanceFromTarget);
            Agent.SetDestination(enemyManager.CurrentTarget.transform.position);

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
    }
}