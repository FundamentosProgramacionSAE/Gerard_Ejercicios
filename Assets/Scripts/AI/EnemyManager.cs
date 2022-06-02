using System;
using System.Collections;
using System.Collections.Generic;
using Player.Manager;
using UnityEngine;
using Random = UnityEngine.Random;
using AI.States;
using AI.Stats;


namespace AI.Manager
{
    public class EnemyManager : CharacterManager
    {
        public List<State> States;
        public State CurrentState;
        
        public VisionEnemy VisionEnemy;
        public Transform visionPosition;
        public bool IsPreformingAction;
        public float StoppingDistance = 0.5f;
        public float RotationSpeed;
        public float CurrentRecoveryTime = 0;
        public float MaxAttackRange = 1.5f;
        public GameObject CurrentTarget;



        public EnemyLocomotionManager EnemyLocomotion;
        private EnemyAnimatorManager _enemyAnimatorManager;
        private EnemyStats _enemyStats;
        private Dictionary<FSMStateType, State> _statesDictionary;

        private void Awake()
        {
            EnemyLocomotion = GetComponent<EnemyLocomotionManager>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            _enemyStats = GetComponent<EnemyStats>();
        }

        private void Start()
        {
            CurrentState = null;
            _statesDictionary = new Dictionary<FSMStateType, State>();

            foreach (var state in States)
            {
                state.SetNavMesh(EnemyLocomotion.Agent);
                state.SetEnemyManager(this);
                _statesDictionary.Add(state.StateType, state);
            }
            EnterState(FSMStateType.IDLE);
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (CurrentState != null)
            {
                CurrentState.UpdateState(this, _enemyStats, _enemyAnimatorManager);
            }
        }

        private void HandleRecoveryTimer()
        {
            if (CurrentRecoveryTime > 0) CurrentRecoveryTime -= Time.deltaTime;

            if (IsPreformingAction)
            {
                if (CurrentRecoveryTime <= 0) IsPreformingAction = false;
            }
        }
        
        /// <summary>
        /// Enter state with State
        /// </summary>
        /// <param name="nextState"></param>
        private void EnterState(State nextState)
        {
            if(nextState == null) return;

            if (CurrentState != null) CurrentState.ExitState();
        
            CurrentState = nextState;
            CurrentState.EnterState();
        }

        /// <summary>
        /// Enter state with FSMStateType
        /// </summary>
        /// <param name="stateType"></param>
        public void EnterState(FSMStateType stateType)
        {
            if (_statesDictionary.ContainsKey(stateType))
            {
                var nextState = _statesDictionary[stateType];

                EnterState(nextState);
            }
        }
        
        public void TargetDetection(FSMStateType fsmStateType)
        {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, VisionEnemy.ViewRadius,
                VisionEnemy.targetMask);
            foreach (var t in targetsInViewRadius)
            {
                var target = t.transform;

                var directionToTarget = (target.position - transform.position).normalized;
                if ((Vector3.Angle(visionPosition.forward, directionToTarget) <
                     VisionEnemy.ViewAngle / 2))
                {
                    var distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (Physics.Raycast(visionPosition.position, directionToTarget,
                            distanceToTarget - VisionEnemy.Offset,
                            VisionEnemy.obstacleMask))
                        continue;

                    if (!target != PlayerManager.Instance.gameObject)
                    {
                        CurrentTarget = target.gameObject;
                        EnterState(fsmStateType);
                    }
                }
            }
        }
        public void StopEnemy()
        {
            _enemyAnimatorManager.Animator.SetFloat("Vertical",0,0.1f,Time.deltaTime);
        }
        
    }
    
}

