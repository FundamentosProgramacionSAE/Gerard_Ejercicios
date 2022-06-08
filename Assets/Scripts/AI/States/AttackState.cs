using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI.States
{
    public class AttackState : State
    {
        public EnemyAttackAction[] EnemyAttacks;
        public EnemyAttackAction CurrentAttack;


        private bool _willDoComboOnNext = false;
        
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.ATTACK;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTER ATTACK STATE");
                Extensions.StopNavMesh(Agent);

            }
        
            return EnteredState;
        }
        
        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //Select on of our many attacks based on attack score
            //If the selected attack is no able to be used because of bad angle or distance, select new attack
            // If Attack is viable, stop our movement and attack our target
            // set out recovery timer to the attacks recovery time
            // return the combat state
            if(enemyManager.IsInteracting && enemyManager.CanDoCombo == false ) return;
            else if (enemyManager.CanDoCombo && enemyManager.IsInteracting)
            {
                if(_willDoComboOnNext)
                {
                    _willDoComboOnNext = false;
                    enemyAnimatorManager.PlayTargetAnimation(CurrentAttack.ActionAnimation, true);
                }
            }
            AttackTarget(enemyManager, enemyAnimatorManager);
        }
        
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING ATTACK STATE");
            return true;
        }
        
        private void AttackTarget(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.CurrentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position,
                enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            
            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);
            
            if (enemyManager.IsPreformingAction)
            {
                enemyManager.EnterState(FSMStateType.COMBAT);
            }

            if (CurrentAttack != null)
            {
                if (distanceFromTarget < CurrentAttack.MaxDistanceToAttack)
                {
                    if (viewableAngle <= CurrentAttack.MaxAttackAngle &&
                        viewableAngle >= CurrentAttack.MinAttackAngle)
                    {
                        if (enemyManager.CurrentRecoveryTime <= 0 && enemyManager.IsPreformingAction == false)
                        {
                            enemyAnimatorManager.Animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.Animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.PlayTargetAnimation(CurrentAttack.ActionAnimation, true);
                            enemyManager.IsPreformingAction = true;
                            RollForComboChance();


                            if (CurrentAttack.CanCombo && _willDoComboOnNext)
                            {
                                CurrentAttack = CurrentAttack.ComboAction;
                                return;
                            }
                            else
                            {
                                enemyManager.CurrentRecoveryTime = CurrentAttack.RecoveryTime;
                                CurrentAttack = null;
                                enemyManager.EnterState(FSMStateType.COMBAT);
                            }
                            
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager,distanceFromTarget);
            }
            enemyManager.EnterState(FSMStateType.COMBAT);
        }
        
        
        private void GetNewAttack(EnemyManager enemyManager, float distanceFromTarget)
        {
            Vector3 targeDireaciton = enemyManager.CurrentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targeDireaciton, transform.forward);

            
            int maxScore = 0;
            
            for (int i = 0; i < EnemyAttacks.Length; i++)
            {
                var enemyAttack = EnemyAttacks[i];
                if (distanceFromTarget <= enemyAttack.MaxDistanceToAttack &&
                    distanceFromTarget >= enemyAttack.MinDistanceToAttack)
                {
                    if (viewableAngle <= enemyAttack.MaxAttackAngle && 
                        viewableAngle >= enemyAttack.MinAttackAngle)
                    {
                        maxScore += enemyAttack.AttackScore;
                    }
                }
            }
            
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            
            for (int i = 0; i < EnemyAttacks.Length; i++)
            {
                var enemyAttack = EnemyAttacks[i];
                if (distanceFromTarget <= enemyAttack.MaxDistanceToAttack &&
                    distanceFromTarget >= enemyAttack.MinDistanceToAttack)
                {
                    if (viewableAngle <= enemyAttack.MaxAttackAngle && 
                        viewableAngle >= enemyAttack.MinAttackAngle)
                    {
                        if(CurrentAttack != null) return;
            
                        temporaryScore += enemyAttack.AttackScore;
            
                        if (temporaryScore > randomValue)
                        {
                            CurrentAttack = enemyAttack;
                        }
                    }
                }
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

        private void RollForComboChance()
        {
            float comboChance = Random.Range(0, 100);

            if (EnemyManager.AllowAIToPerformCombos && comboChance <= EnemyManager.ComboChance)
            {
                _willDoComboOnNext = true;
            }
        }
    }
}