using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoIcon : AbstractAbilityIcon
{
    protected override void Awake()
    {
        base.Awake();
        ability = player.GetComponent<Camo>();
    }
    protected override void Start()
    {
        base.Start();
        if (!GameManager.instance.HasCamo) abilityIconImage.gameObject.SetActive(false);
        else abilityIconImage.gameObject.SetActive(true);
    }
}
