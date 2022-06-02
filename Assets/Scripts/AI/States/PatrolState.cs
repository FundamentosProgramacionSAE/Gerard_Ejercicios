using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI.States
{
    public class PatrolState : State
    {
        public WaypointSystem WaypointSystem;
        public bool Recursive;
        public int CurrentPosition = -1;
        bool GettingBack;
        private bool FirstFrame;

        private Vector3 _position;
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.PATROL;
            CurrentPosition = -1;
        }
        
        public override bool EnterState()
        {
            EnteredState = base.EnterState();
            if (EnteredState)
            {
                if (WaypointSystem != null)
                {
                    // Set waypoint and position Agent
                    SetPositionIndex();
                    FirstFrame = true;
                }

            }
        
            return EnteredState;
        }
        public override void UpdateState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            EnemyManager.EnemyLocomotion.HandleMoveToPosition(SetNewPosition(), 0.5f);
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
                CurrentPosition = (CurrentPosition + 1) % WaypointSystem.Waypoints.Count;
            }
            else
            {
                if (CurrentPosition == WaypointSystem.Waypoints.Count - 1 && !GettingBack )
                {
                    GettingBack = true;
                }
                else if(GettingBack && CurrentPosition == 0)
                {
                    GettingBack = false;
                }

                if (GettingBack)
                {
                    CurrentPosition = (CurrentPosition - 1) % WaypointSystem.Waypoints.Count;
                }
                else
                {
                    CurrentPosition = (CurrentPosition + 1) % WaypointSystem.Waypoints.Count;
                }
            }
        }
    
        /// <summary>
        /// Posicion del waypoint
        /// </summary>
        /// <returns>SetNewPosition</returns>
        private Vector3 SetNewPosition()
        { 
            _position = WaypointSystem.Waypoints[CurrentPosition].position;
            return _position;
        }
    }
}