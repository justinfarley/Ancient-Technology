using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public bool hasTeleport, hasCamo, hasTimeSlow, hasAttack;
    public int bestLevel;
    public SaveData(GameManager manager)
    {
        bestLevel = manager.bestLevel;
        hasTeleport = manager.HasTeleport;
        hasCamo = manager.HasCamo;
        hasTimeSlow = manager.HasTimeSlow;
        hasAttack = manager.HasAttack;
    }
}
