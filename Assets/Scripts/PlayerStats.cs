using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    internal List<Upgrades> unlockedUpgrades = new List<Upgrades>();
    public PlayerData(List<Upgrades> unlockedUpgrades)
    {
        this.unlockedUpgrades = unlockedUpgrades;
    }
}
