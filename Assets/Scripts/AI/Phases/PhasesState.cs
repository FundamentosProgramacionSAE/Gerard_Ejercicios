using System;
using AI.Manager;
using AI.Stats;
using UnityEngine;

namespace AI
{
    public class PhasesState : MonoBehaviour
    {
        public PhasesBossManager PhasesBossManager;
        public Phases Phase;

        private float _currentTime;
        private EnemyStats _enemyStats;
        private bool _firstTime = true;

        private void Awake()
        {
            _enemyStats = GetComponentInParent<EnemyStats>();
        }

        private void Start()
        {
           //ResetCooldown();
        }

        public void Update()
        {
            Debug.LogWarning(_enemyStats.healthSystem.PercentHealth());
            if(_enemyStats.healthSystem.PercentHealth() > Phase.HealthPercent) return;

            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                PhasesBossManager.SetCurrentPhase(this);

                if (_firstTime)
                {
                    PhasesBossManager.SetupParameters(this);
                    _firstTime = false;
                }
                
                ResetCooldown();
            }
        }

        public void ResetCooldown()
        {
            _currentTime = Phase.Cooldown;
        }
    }
}