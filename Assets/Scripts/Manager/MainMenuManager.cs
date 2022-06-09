using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace  Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        public Button NewGameButton;
        public Button ContinueGameButton;
        private void Start()
        {
            var exists = ES3.FileExists();
            ContinueGameButton.gameObject.SetActive(exists);


            
            NewGameButton.onClick.AddListener(NewGame);
            ContinueGameButton.onClick.AddListener(ContinueGame);
        }
        
        private void NewGame()
        {
            var newInventory = new InventorySystem();
            var myInventory = ES3.Load("Inventory", newInventory.ItemsDictionary);

            foreach (var item in myInventory)
            {
                item.Key.Position = -1;

                var weapon = item.Key as WeaponItem;
                if (weapon != null) weapon.IsUsed = false;
            }
            ES3.DeleteFile("Player.es3");
            SceneManager.LoadSceneAsync(1);
        }

        private void ContinueGame()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}

