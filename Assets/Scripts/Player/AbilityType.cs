
using UnityEngine;

namespace Ability.Type
{
    [CreateAssetMenu(menuName = "Ability/Type")]
    public class AbilityType : ScriptableObject
    {
        public GameObject PrefabHitParticles;
        public Sprite SpriteImage;
        public float Cooldown;
        public int DamagePercentToAdd;
    }
}


