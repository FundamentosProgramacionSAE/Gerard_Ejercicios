using System;
using System.Collections.Generic;
using AI.States;
using AI.Stats;
using UnityEngine;
using  Sirenix.OdinInspector;

namespace AI.Manager
{
    public class PhasesBossManager : SerializedMonoBehaviour
    {
        public List<PhasesState> PhasesList = new List<PhasesState>();
        public AttackBossState AttackState;
        public Phases CurrentPhase;
        
        private EnemyStats _enemyStats;
        private EnemyAnimatorManager _enemyAnimatorManager;

        private void Awake()
        {
            _enemyStats = GetComponent<EnemyStats>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        public void SetCurrentPhase(PhasesState phasesState)
        {
            CurrentPhase = phasesState.Phase;
            
            foreach (var phases in PhasesList)
            {
                phases.ResetCooldown();
            }
            StartPhase();
        }

        public void SetupParameters(PhasesState phasesState)
        {
            _enemyStats.DamageToAdd += phasesState.Phase.DamageAdded;
            _enemyStats.Defense += phasesState.Phase.DefenseAdded;
            _enemyStats.SpeedToAdd += phasesState.Phase.VelocityAdded;
            _enemyAnimatorManager.PlayTargetAnimation(phasesState.Phase.AnimationPhase, true);
        }

        private void StartPhase()
        {
            AttackState.PhaseAttack = CurrentPhase.AttackAction;
        }
            
    }
}