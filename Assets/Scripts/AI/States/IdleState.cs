using System;
using AI.Manager;
using AI.Stats;
using Player.Manager;
using UnityEngine;

namespace AI.States
{
    public class IdleState : State
    {
        
        [SerializeField, Tooltip("Tiempo para pasar el siguiente estado")] float TimeToNext = 1f;
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
                Extensions.StopNavMesh(Agent);
                _currentTime = 0f;
            }

            return EnteredState;
        }

        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // Comprueba si el enemigo puede ver al jugador
            enemyManager.TargetDetection(FSMStateType.CHASE);
            enemyManager.StopEnemy(); // Paramdos la animacion del enemigo
            
            if(enemyManager.IsBoss) return;
            _currentTime += Time.deltaTime;
            // When current time arrived in TimeToNext. PATROL
            if (_currentTime > TimeToNext)
            {
                enemyManager.EnterState(FSMStateType.PATROL);
            }
        }
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXIT IDLE STATE");
            return true;

        }

        
    }
}