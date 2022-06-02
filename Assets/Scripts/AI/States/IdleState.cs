using System;
using AI.Manager;
using AI.Stats;
using Player.Manager;
using UnityEngine;

namespace AI.States
{
    public class IdleState : State
    {
        
        [SerializeField] float TimeToNext = 1f;
        private float _currentTime;
        
        
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.IDLE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                print("ENTERED IDLE STATE");
                _currentTime = 0f;
            }

            return EnteredState;
        }

        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // Look for potential target
            enemyManager.TargetDetection(FSMStateType.CHASE);
            enemyManager.StopEnemy();
            
            _currentTime += Time.deltaTime;
            // When current time arrived in TimeToNext. PATROL
            if (_currentTime > TimeToNext)
            {
                enemyManager.EnterState(FSMStateType.PATROL);
            }
            // Switch to follow target
            // If not return the state
        }
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXIT IDLE STATE");
            return true;

        }

        
    }
}