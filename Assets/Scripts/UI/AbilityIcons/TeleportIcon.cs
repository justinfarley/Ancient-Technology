using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportIcon : AbstractAbilityIcon
{
    protected override void Awake()
    {
        base.Awake();
        ability = player.GetComponent<Teleport>();
    }
    protected override void Start()
    {
        base.Start();
        abilityIconImage.gameObject.SetActive(false);
    }
}
