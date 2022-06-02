using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI.States
{
    public class AttackState : State
    {
        public EnemyAttackAction[] EnemyAttacks;
        public EnemyAttackAction CurrentAttack;
        
        
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
                            enemyManager.CurrentRecoveryTime = CurrentAttack.RecoveryTime;
                            CurrentAttack = null;
                            enemyManager.EnterState(FSMStateType.COMBAT);
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
    }
}