using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action", fileName = "Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public bool CanCombo;
        public EnemyAttackAction ComboAction;
        
        public int AttackScore = 3;
        public float RecoveryTime = 2;

        public float MaxAttackAngle = 35;
        public float MinAttackAngle = -35;

        public float MinDistanceToAttack = 0;
        public float MaxDistanceToAttack = 3;

    }
}