using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [Header("File Storage Config")] 
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    
    
    
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;
    private GameData _gameData;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Se ha encontrado mas de una Data Persistente en la escena. Destryuendo la nueva.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }


    private void Start()
    {
        
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();
        
        
        if (_gameData == null)
        {
            Debug.Log("No se ha encontrado ninguna data que cargar, se creara uno con lo Default");
            NewGame();
            // Debug.Log("No se ha encontrado ninguna data que cargar, porfavor inicie una partida");
            // return;
        }

        foreach (var dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(_gameData);
        }
        print($"Se ha cargado correctamente la data del juego \n" +
              $"Nombre del archivo: {fileName} \n" +
              $"Ruta del archivo {fileName}: {Application.persistentDataPath}.");
    }

    public void SaveGame()
    {

        foreach (var dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(_gameData);
        }
        
        _dataHandler.Save(_gameData);
        
        print($"Se ha guardado correctamente la data del juego \n" +
              $"Nombre del archivo: {fileName} \n" +
              $"Ruta del archivo {fileName}: {Application.persistentDataPath}.");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Se ha cargado la escena {scene.name}, para que cargue lo guardado.");
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Guardando datos al no cargar la escena...");
        SaveGame();
    }
    

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData => _gameData != null;
}
