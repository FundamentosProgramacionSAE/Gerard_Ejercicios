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
                EnemyManager.transform.LookAt(EnemyManager.CurrentTarget.transform);

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

            float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position,
                enemyManager.transform.position);

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
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING COMBAT STATE");
            return true;
        }
    }
}