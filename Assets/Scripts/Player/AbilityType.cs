
using UnityEngine;

namespace Ability.Type
{
    [CreateAssetMenu(menuName = "Ability/Type")]
    public class AbilityType : ScriptableObject
    {
        public GameObject PrefabStartParticles;
        public GameObject PrefabHitParticles;
        public Sprite SpriteImage;
        public float Cooldown;
    }
}


