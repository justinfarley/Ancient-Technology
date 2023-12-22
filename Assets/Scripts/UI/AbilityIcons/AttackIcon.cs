using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AttackIcon : AbstractAbilityIcon
{
    protected override void Awake()
    {
        base.Awake();
        ability = player.GetComponent<Attack>();
    }
    protected override void Start()
    {
        base.Start();
        if (!GameManager.instance.HasAttack) abilityIconImage.gameObject.SetActive(false);
        else abilityIconImage.gameObject.SetActive(true);
    }
}
