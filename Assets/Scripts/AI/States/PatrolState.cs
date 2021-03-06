using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI.States
{
    public class PatrolState : State
    {
        public WaypointSystem WaypointSystem;
        [Tooltip("Siver para si el enemigos al finalzar el waypoint vuelve al anterior o el inicio")]
        public bool Recursive;
        
        
        private int _currentPosition = -1;
        private bool _gettingBack;
        private Vector3 _position;
        
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.PATROL;
            _currentPosition = -1;
        }
        
        public override bool EnterState()
        {
            EnteredState = base.EnterState();
            if (EnteredState)
            {
                if (WaypointSystem != null)
                {
                    // Set waypoint and position Agent
                    Extensions.PlayNavMesh(Agent);
                    Agent.speed = EnemyManager.WalkSpeed;
                    SetPositionIndex();
                }

            }
        
            return EnteredState;
        }
        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // Movemos la Ia al punto del waypoint
            if(WaypointSystem != null) HandleMoveToPosition(SetNewPosition(), 0.5f, enemyManager, enemyAnimatorManager);
            float distanceFromTarget = Vector3.Distance(_position,
                enemyManager.transform.position);
            
            if (WaypointSystem == null)
            {
                enemyManager.EnterState(FSMStateType.IDLE);
                return;
            }
            enemyManager.TargetDetection(FSMStateType.CHASE);

            if (distanceFromTarget <= enemyManager.StoppingDistance)
            {
                enemyManager.EnterState(FSMStateType.IDLE);
            }
        }
        
        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXIT PATROL STATE");
            return true;

        }
        
        /// <summary>
        /// Funcion para el waypoint donde tiene que ir
        /// </summary>
        private void SetPositionIndex()
        {
            if (Recursive)
            {
                _currentPosition = (_currentPosition + 1) % WaypointSystem.Waypoints.Count;
            }
            else
            {
                if (_currentPosition == WaypointSystem.Waypoints.Count - 1 && !_gettingBack )
                {
                    _gettingBack = true;
                }
                else if(_gettingBack && _currentPosition == 0)
                {
                    _gettingBack = false;
                }

                if (_gettingBack)
                {
                    _currentPosition = (_currentPosition - 1) % WaypointSystem.Waypoints.Count;
                }
                else
                {
                    _currentPosition = (_currentPosition + 1) % WaypointSystem.Waypoints.Count;
                }
            }
        }
    
        /// <summary>
        /// Posicion del waypoint
        /// </summary>
        /// <returns>SetNewPosition</returns>
        private Vector3 SetNewPosition()
        { 
            _position = WaypointSystem.Waypoints[_currentPosition].position;
            return _position;
        }
        
        public void HandleMoveToPosition(Vector3 CurrentTarget, float valueSpeed, EnemyManager _enemyManager, EnemyAnimatorManager _enemyAnimatorManager)
        {
            if(_enemyManager.IsPreformingAction) return;

            Vector3 targetDirection = CurrentTarget - transform.position;
            float distanceFromTarget = Vector3.Distance(CurrentTarget, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            
            if (distanceFromTarget > _enemyManager.StoppingDistance)
            {
                _enemyAnimatorManager.Animator.SetFloat("Vertical", valueSpeed, 0.1f, Time.deltaTime);
            }
            else if(distanceFromTarget <= _enemyManager.StoppingDistance)
            {
                _enemyAnimatorManager.Animator.SetFloat("Vertical",0,0.1f,Time.deltaTime);
            }
            
            
            Agent.SetDestination(CurrentTarget);
            
        }
    }
}