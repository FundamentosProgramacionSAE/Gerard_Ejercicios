using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Inventory.Item
{
    public class Item : ScriptableObject
    {
        [Title("Item Information")] 
        public Sprite ItemIcon;
        public string ItemName;
    }
}


