using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject ModelPrefab;
        public int WeaponDamage;
        public bool IsUnarmed;
        
        [Title("Attack Animations")]
        public string OHLightAttack1;
        public string OHLightAttack2;
        public string OHLightAttack3;
        public string OHLightAttack4;
        public string AbilityAttack2;
        public string AbilityAttack3;
        public string AbilityAttack4;

    }
}


