using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileDataHandler
{
    public static  readonly byte[] KEY = Convert.FromBase64String("AQ/XXzHZjAaqgTKZKnR7L/xEYwARa9443JbAfKZWnhE=");

    private string _dataDirPath = "";
    private string _dataFileName = "";
    private bool _useEncryption = false;
    
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
        _useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
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

                if (_useEncryption)
                {
                    dataToLoad = Decrypt(dataToLoad, true, Convert.ToBase64String(KEY,0,KEY.Length));
                }
                
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ha ocurrido un error cargando la data: {fullPath}\n{e}");
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_useEncryption)
            {
                dataToStore =Encrypt(dataToStore, true, Convert.ToBase64String(KEY,0,KEY.Length));
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ha ocurrido un error guardando la data: {fullPath}\n{e}");
        }
    }


    // private string EncryptDecrypt(string data)
    // {
    //     string modifiedData = "";
    //
    //     for (int i = 0; i < data.Length; i++)
    //     {
    //         modifiedData += (char) (data[i] ^ KEY[i % KEY.Length]);
    //     }
    //
    //     return modifiedData;
    // }
    
    
    public static string Encrypt(string toEncrypt, bool useHashing, string SecurityKey)
    {
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        string key = SecurityKey;
        //If hashing use get hashcode regards to your key
        if (useHashing)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
        }
        else
            keyArray = UTF8Encoding.UTF8.GetBytes(key);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //set the secret key for the tripleDES algorithm
        tdes.Key = keyArray;
        //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
        tdes.Mode = CipherMode.ECB;
        //padding mode(if any extra byte added)
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        //transform the specified region of bytes array to resultArray
        byte[] resultArray = cTransform.TransformFinalBlock
            (toEncryptArray, 0, toEncryptArray.Length);
        //Release resources held by TripleDes Encryptor
        tdes.Clear();
        //Return the encrypted data into unreadable string format
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    
    public static string Decrypt(string cipherString, bool useHashing, string SecurityKey)
    {
        byte[] keyArray;

        byte[] toEncryptArray = Convert.FromBase64String(cipherString);
        string key = SecurityKey;

        if (useHashing)
        {
            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();
        }
        else
        {
            //if hashing was not implemented get the byte code of the key
            keyArray = UTF8Encoding.UTF8.GetBytes(key);
        }

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //set the secret key for the tripleDES algorithm
        tdes.Key = keyArray;
        //mode of operation. there are other 4 modes.
        tdes.Mode = CipherMode.ECB;
        //padding mode(if any extra byte added)
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock
            (toEncryptArray, 0, toEncryptArray.Length);
        //Release resources held by TripleDes Encryptor
        tdes.Clear();
        //return the Clear decrypted TEXT
        return UTF8Encoding.UTF8.GetString(resultArray);
    }

}
