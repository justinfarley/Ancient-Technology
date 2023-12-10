using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unlockable : MonoBehaviour
{
    protected bool locked = true;
    protected int id;
    [SerializeField] protected string upgradeName;
    protected Action OnUnlock;
    public virtual void Awake()
    {
        OnUnlock += () => locked = false;
        OnUnlock += () => print("Unlocked " + upgradeName);
    }
    public virtual void Start()
    {
        OnUnlock += () => GameManager.SaveGame();
    }
    public virtual void Unlock()
    {
        if (!locked) return; 
        OnUnlock?.Invoke();
    }
    public bool IsLocked()
    {
        return locked;
    }
}
