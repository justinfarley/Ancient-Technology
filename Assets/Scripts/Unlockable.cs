using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unlockable : MonoBehaviour
{
    private bool locked = true;
    protected int id;
    [SerializeField] protected string upgradeName;
    protected Action OnUnlock;
    public virtual void Start()
    {
        OnUnlock += () => locked = false;
        OnUnlock += () => print("Unlocked " + upgradeName);
    }
    public void Unlock()
    {
        OnUnlock?.Invoke();
    }
    public bool IsLocked()
    {
        return locked;
    }
}
