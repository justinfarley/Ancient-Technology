using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/MAYANÆÝ※‡⅏.ERR";
    //private static string path = Application.persistentDataPath + "/GAUNTLETSUPERPOWERS.ERR";

    public static void SaveData()
    {
        Debug.Log("Saving Data...");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(GameManager.instance);
        formatter.Serialize(fs, data);
        fs.Close();
        Debug.Log("Saved Successfully!");
    }
    public static SaveData LoadData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(fs) as SaveData;
            fs.Close();
            return data;
        }
        else
        {
            GameManager.hasData = false;
            Debug.Log("not found in" + path);
            return null;
        }
    }
    public static void DeleteAllData()
    {
        if (File.Exists(path))
        {
            Debug.Log("deleted file");
            File.Delete(path);
            //UnityEditor.AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log("file doesnt exist");
        }
    }
    public static bool HasData()
    {
        return File.Exists(path);
    }
}
