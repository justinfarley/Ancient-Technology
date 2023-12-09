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
        if(SaveSystem.HasData())
            LoadGame();
    }
    private void OnLevelWasLoaded(int level)
    {
        if(level > bestLevel)
            bestLevel = level;
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
            instance.SaveGame();
        }
    }
    public void DeleteData()
    {
        SaveSystem.DeleteAllData();
        bestLevel = 0;
        hasTeleport = false;
        hasCamo = false;
        hasTimeSlow = false;
        hasAttack = false;
    }
    public void SaveGame()
    {
        SaveSystem.SaveData();
    }
    public SaveData LoadGame()
    {
        SaveData loadedData = SaveSystem.LoadData();
        if (loadedData == null) return null;
        hasTeleport = loadedData.hasTeleport;
        hasCamo = loadedData.hasCamo;
        hasTimeSlow = loadedData.hasTimeSlow;
        hasAttack = loadedData.hasAttack;
        bestLevel = loadedData.bestLevel;
        OnLoadedGame?.Invoke();
        return loadedData;
    }
    public bool HasTeleport { get { return hasTeleport; } set { hasTeleport = value; } }
    public bool HasCamo { get { return hasCamo; } set { hasCamo = value; } }
    public bool HasTimeSlow { get {  return hasTimeSlow; } set { hasTimeSlow = value; } }
    public bool HasAttack { get { return hasAttack; } set { hasAttack = value; } }
}
