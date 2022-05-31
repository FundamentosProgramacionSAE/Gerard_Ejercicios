using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string ID;
    public string DisplayName;
    public string InfoItem;
    public Sprite Icon;
    public MinMaxInt RandomAmounts = new MinMaxInt{Min = 1, Max = 2};
    public int MaxAmount = 10;
    public int Position = -1;

}

