using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeShiftIcon : AbstractAbilityIcon
{
    protected override void Awake()
    {
        base.Awake();
        ability = player.GetComponent<TimeShift>();
    }
    protected override void Start()
    {
        base.Start();
        if (!GameManager.instance.HasTimeSlow) abilityIconImage.gameObject.SetActive(false);
        else abilityIconImage.gameObject.SetActive(true);
    }
}
