using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CharacterType{Warrior, Mage, Assassin, Archer}
public class Player : MonoBehaviour, IDataPersistence
{
    public Vector3 Position;
    public string NameCharacter;
    public CharacterType CharacterType;
    public Color Color;
    public List<string> Names = new List<string>{"A", "B", "C", "D"};


    private void Start()
    {
        Names.Shuffle();
        Extensions.DebugColor("HOLA", "orange");
    }

    public void LoadData(GameData data)
    {
        Position = data.Position;
        NameCharacter = data.NameCharacter;
        CharacterType = data.CharacterType;
        Color = Extensions.GetColorFromString(data.Color);
    }

    public void SaveData(GameData data)
    {
        data.Position = Position;
        data.NameCharacter = NameCharacter;
        data.CharacterType = CharacterType;
        data.Color = Extensions.GetStringFromColor(Color);
    }
}
