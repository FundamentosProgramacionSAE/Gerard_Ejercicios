using Sirenix.OdinInspector;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    [TitleGroup("STATS")]
    public int HealthLevel = 10;
    public int MaxHealth;
    public int Defense;
    public int DamageToAdd;
    public int SpeedToAdd;
    public HealthSystem healthSystem;

}
