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
        /// <summary>
        /// Estado de ejecucion del estado
        /// </summary>
        private ExecutionState ExecutionState { get; set; }
        
        /// <summary>
        /// Tipo de estado de la IA
        /// </summary>
        public FSMStateType StateType { get; protected set; }
        
        /// <summary>
        /// Bool para comprobar si entra en el estado o no
        /// </summary>
        protected bool EnteredState { get; set; }
        
        /// <summary>
        /// Agente de la IA
        /// </summary>
        protected NavMeshAgent Agent { get; set; }
        
        /// <summary>
        /// Manager del enemigo
        /// </summary>
        protected EnemyManager EnemyManager { get; set; }
        
        
        /// <summary>
        /// Funcion que sera llamada en el update del EnemyManager.cs
        /// </summary>
        /// <param name="enemyManager"></param>
        /// <param name="enemyStats"></param>
        /// <param name="enemyAnimatorManager"></param>
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
        public void SetNavMesh(NavMeshAgent agent)
        {
            Agent = agent;
        }
        
        /// <summary>
        /// Add component Enemy Manager
        /// </summary>
        /// <param name="agent"></param>
        public void SetEnemyManager(EnemyManager enemyManager)
        {
            EnemyManager = enemyManager;
        }

    }
}


