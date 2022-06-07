using System.Collections;
using System.Collections.Generic;
using Ability.Type;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : ItemData
    {
        public GameObject ModelPrefab;
        public int WeaponDamage;
        public MinMaxInt DamageToAdd;
        public float CriticalDamageMultiplier = 1.5f;
        [Range(0, 100)] public int CriticalRate;
        public MinMaxInt PhysicalDamageAbsorption; 
        public bool IsUnarmed;
        public bool IsDualWeapon;
        public bool IsShield;
        public bool IsUsed;
        
        [Title("Attack Animations")]
        public string OHLightAttack1;
        public string OHLightAttack2;
        public string OHLightAttack3;
        public string OHLightAttack4;
        public string AbilityAttack2;
        public string AbilityAttack3;
        public string AbilityAttack4;

        [Title("Ability Types")] 
        public AbilityType AbilityType2;        
        public AbilityType AbilityType3;
        public AbilityType AbilityType4;



    }
}


