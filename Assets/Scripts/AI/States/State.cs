using System.Collections;
using System.Collections.Generic;
using AI.Manager;
using AI.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public enum ExecutionState
    {
        NONE,
        ACTIVE,
        COMPLETED,
        TERMINATED,
    }

    public enum FSMStateType
    {
        IDLE,
        PATROL,
        CHASE,
        COMBAT,
        ATTACK,
        DIE
    }
    public abstract class State : MonoBehaviour
    {
        private ExecutionState ExecutionState { get; set; }
        public FSMStateType StateType { get; protected set; }
        protected bool EnteredState { get; set; }
        protected NavMeshAgent Agent { get; set; }
        protected EnemyManager EnemyManager { get; set; }
        
        public abstract void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager);
        
        
        /// <summary>
        /// When enable States, Only activate when Play
        /// </summary>
        public virtual void OnEnable()
        {
            ExecutionState = ExecutionState.NONE;
        }
        
        /// <summary>
        /// When state enter
        /// </summary>
        /// <returns>true</returns>
        public virtual bool EnterState()
        {
            ExecutionState = ExecutionState.ACTIVE;
            return true;
        }
        
        /// <summary>
        /// When state exit and goes to new state
        /// </summary>
        /// <returns>true</returns>
        public virtual bool ExitState()
        {
            ExecutionState = ExecutionState.COMPLETED;
            return true;
        }
        
        /// <summary>
        /// Add component NavMeshAgent
        /// </summary>
        /// <param name="agent"></param>
        public virtual void SetNavMesh(NavMeshAgent agent)
        {
            Agent = agent;
        }
        
        /// <summary>
        /// Add component Enemy MAnager
        /// </summary>
        /// <param name="agent"></param>
        public virtual void SetEnemyManager(EnemyManager enemyManager)
        {
            EnemyManager = enemyManager;
        }

    }
}


