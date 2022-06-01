using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    public InventorySystem InventorySystem;
    private void Start()
    {
        InventorySystem.ItemsDictionary = ES3.Load("Inventory", InventorySystem.ItemsDictionary);
        InventorySystem.Gold = ES3.Load("Gold", InventorySystem.Gold);
        InventorySystem.Initialized();
    }
    

    public void OnApplicationQuit()
    {
        ES3.Save("Inventory", InventorySystem.ItemsDictionary);
        ES3.Save("Gold", InventorySystem.Gold);
    }
}

