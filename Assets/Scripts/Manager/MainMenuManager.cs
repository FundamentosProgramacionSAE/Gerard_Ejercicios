using System;
using System.Collections;
using System.Collections.Generic;
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
            ES3.DeleteFile();
            SceneManager.LoadSceneAsync(1);
        }

        private void ContinueGame()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}

