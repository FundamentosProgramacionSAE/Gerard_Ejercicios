using Managers;
using Player.Locomotion;
using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(menuName = "Items/Flask Item")]
    public class FlaskItem : ItemData
    {
        public MinMaxInt HealthRecoverAmount;
        public GameObject RecoverFX;
        public int TimeToUse;
    }
}