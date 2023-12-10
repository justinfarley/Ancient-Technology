using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool hasData = false;
    public int bestLevel = 0;
    private bool hasTeleport, hasCamo, hasTimeSlow, hasAttack;
    public static Action OnLoadedGame;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        print("LOADING");
        LoadGame();
        print(instance.hasTeleport);
    }
    private void OnLevelWasLoaded(int level)
    {
        LoadGame();
        if(level > bestLevel)
            instance.bestLevel = level;
        SaveGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            SaveSystem.DeleteAllData();
        }
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            GameManager.SaveGame();
        }
    }
    public static void DeleteData()
    {
        SaveSystem.DeleteAllData();
        instance.bestLevel = 0;
        instance.hasTeleport = false;
        instance.hasCamo = false;
        instance.hasTimeSlow = false;
        instance.hasAttack = false;
    }
    public static void SaveGame()
    {
        SaveSystem.SaveData();
    }
    public static SaveData LoadGame()
    {
        if (!SaveSystem.HasData()) return null;
        SaveData loadedData = SaveSystem.LoadData();
        if (loadedData == null) return null;
        instance.hasTeleport = loadedData.hasTeleport;
        instance.hasCamo = loadedData.hasCamo;
        instance.hasTimeSlow = loadedData.hasTimeSlow;
        instance.hasAttack = loadedData.hasAttack;
        instance.bestLevel = loadedData.bestLevel;
        OnLoadedGame?.Invoke();
        return loadedData;
    }
    public bool HasTeleport { get { return hasTeleport; } set { hasTeleport = value; } }
    public bool HasCamo { get { return hasCamo; } set { hasCamo = value; } }
    public bool HasTimeSlow { get {  return hasTimeSlow; } set { hasTimeSlow = value; } }
    public bool HasAttack { get { return hasAttack; } set { hasAttack = value; } }
}
