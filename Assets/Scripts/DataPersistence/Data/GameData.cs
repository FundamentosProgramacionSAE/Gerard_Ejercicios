using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 Position;
    public string NameCharacter;
    public SerializableDictionary<string, string> ItemsDictionary;
    public CharacterType CharacterType;
    public string Color;


    public GameData()
    {
        Position = Vector3.zero;
        NameCharacter = "";
        ItemsDictionary = new SerializableDictionary<string, string>();
        CharacterType = CharacterType.Warrior;
        Color = "FFFFFF";
    }
}
