using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Dictionaries : MonoBehaviour
{
    public InputField InputField;
    public Button SendButton;

    public SerializableDictionary<string,int> RecordTable;


    private void Awake()
    {
        SendButton.onClick.AddListener(() =>
        {
            if(InputField.text == null) return;
            
            var textField = InputField.text;
            if(RecordTable.ContainsKey(textField) == false) return;

            RecordTable[textField] = 90;
            print(GetNameWithKey(textField) + GetValueWithKey(textField));
        });
    }

    /// <summary>
    /// Obtener el nombre y el valor por la key de un diccionario
    /// </summary>
    /// <param name="textField"></param>
    private string GetNameWithKey(string textField)
    {
        var key = RecordTable.FirstOrDefault(x => x.Key == textField);
        return key.Key;
    }

    private int GetValueWithKey(string textField)
    {
        var key = RecordTable.FirstOrDefault(x => x.Key == textField);
        return key.Value;
    }

    private void Start()
    {
        RecordTable = new SerializableDictionary<string, int>
        {
            {"Natalia", 8},
            {"Gerard", 10},
            {"Sergi", 9}
        };
    }
    
}
