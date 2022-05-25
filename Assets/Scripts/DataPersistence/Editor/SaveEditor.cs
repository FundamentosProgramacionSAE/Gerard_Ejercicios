#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;

public class SaveEditor : EditorWindow
{
    
    public static SaveEditor Instance { get; private set; }
    
    public string Key;
    public string PathDirection;
    public string FileName = "data.game";

    private bool existFile;
    private bool correctKey;

    private GUIStyle style;
    private string DataDecrypted;
    private Vector2 scroll;
    private Rect overSize = new Rect(637,126,455,576);
    
    [MenuItem("Extensions/Save Editor", false)]
    public static void Open()
    {
        SaveEditor window = GetWindow<SaveEditor>(true, "Decrypt Save File", true);
        if(Instance != null) Instance.Close();

        Instance = window;
        window.minSize = new Vector2(Instance.overSize.width, Instance.overSize.height);
        window.maxSize = new Vector2(Instance.overSize.width, Instance.overSize.height);
        window.Show();

    }

    private void OnEnable()
    {
        PathDirection = Application.persistentDataPath;
        Key = PlayerPrefs.GetString("Key");
        FileName = PlayerPrefs.GetString("FileName");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("Key", Key);
        PlayerPrefs.SetString("FileName", FileName);
    }

    private void OnGUI()
    {
        style = new GUIStyle(EditorStyles.label);
        style.richText = true;
        position = new Rect(position.x, position.y, overSize.width, overSize.height);

        EditorStyles.label.richText = true;

        GUILayout.BeginVertical("box");
        
        GUILayout.Label("Key");
        GUILayout.BeginHorizontal();
        Key = GUILayout.TextField(Key);
        if (GUILayout.Button("Take Key"))
        {
            Key = Convert.ToBase64String(FileDataHandler.KEY);
        }
        GUILayout.EndHorizontal();
        

        GUILayout.Label("Path");
        GUILayout.BeginHorizontal();
        PathDirection = GUILayout.TextArea(PathDirection);
        if (GUILayout.Button("Show In Explorer"))
        {
            EditorUtility.RevealInFinder(PathDirection);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Label("File Name");
        FileName = GUILayout.TextField(FileName);
        
        GUILayout.BeginVertical("box");
        GUILayout.Label("Messages", EditorStyles.label);
        switch (existFile)
        {
            case true:
                GUILayout.Label($" Exist File: <color=green>{existFile.ToString()}</color>",style);
                break;
            default:
                GUILayout.Label($" Exist File: <color=red>{existFile.ToString()}</color>",style);
                break;
        }

        switch (correctKey)
        {
            case true:
                GUILayout.Label($" Correct Key: <color=green>{correctKey.ToString()}</color>", style);
                break;
            default:
                GUILayout.Label($" Correct Key: <color=red>{correctKey.ToString()}</color>", style);
                break;
        }
        GUILayout.EndVertical();

        GUILayout.EndVertical();
        
        GUILayout.BeginHorizontal("box");
        
        string fullPath = Path.Combine(PathDirection, FileName);
        
        if (GUILayout.Button("Check File"))
        {

            existFile = (File.Exists(fullPath));
            if (existFile ==false)
            {
                Debug.LogError("No existe el archivo en la ruta, asegurate que el nombre es el correcto o esta en la ruta. " +
                               "Para crear el archivo, juega.");
                var info = new DirectoryInfo(PathDirection);
                var fileInfo = info.GetFiles();
                foreach (var file in fileInfo)
                {
                    Debug.LogWarning($"File in path→ {file.Name}");
                }
            }
        }

        if (GUILayout.Button("Create Key"))
        {
            Aes aes = Aes.Create();
            aes.GenerateKey();
            Debug.Log($"Asigna la nueva clave en el script FileDataHandler. NewKey→{Convert.ToBase64String(aes.Key)}");
        }


        if (existFile)
        {
            if (GUILayout.Button("Decrypt File"))
            {
                try
                {
                    string dataToLoad = "";
                
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    byte[] KEY = Convert.FromBase64String(Key);

                    dataToLoad = FileDataHandler.Decrypt(dataToLoad, true, Convert.ToBase64String(KEY, 0, KEY.Length));
                    DataDecrypted = dataToLoad;
                    correctKey = true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ha ocurrido un error cargando la data: {fullPath}\n{e}");
                    correctKey = false;
                }
            }

            if (GUILayout.Button("Delete File"))
            {
                if(EditorUtility.DisplayDialog("Delete Save File", $"Estas seguro que quieres eliminiar <{FileName}>?",
                    "Ok", "Cancel"))
                {
                    File.Delete(fullPath);
                }

            }

        }
        GUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Decrypt Text");
        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.TextArea(DataDecrypted,GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
        EditorGUILayout.Space(5);
        

    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
    

}

#endif
