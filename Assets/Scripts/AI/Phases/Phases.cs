using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Boss/Phases")]
    public class Phases : ScriptableObject
    {
        public int HealthPercent;
        public float Cooldown = 10;
        public int DamageAdded;
        public int DefenseAdded;
        public int VelocityAdded;
        public string AnimationPhase;
        public EnemyAttackAction AttackAction;
        public GameObject SpawnVFX;
    }
}