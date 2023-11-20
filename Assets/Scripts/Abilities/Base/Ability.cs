using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : Unlockable
{
    [SerializeField]
    protected float abilityCooldown;
    protected bool canUseAbility = false;
    private bool onCooldown = false;
    [SerializeField] private List<KeyCode> abilityKeys;
    private GameObject abilityUIIcon;
    [SerializeField] protected AbstractAbilityIcon abilityIcon;

    public override void Start()
    {
        base.Start();
        abilityUIIcon = abilityIcon.gameObject;
        OnUnlock += () => abilityUIIcon.SetActive(true);
    }
    private bool PressedActionButtonDown()
    {
        foreach(var key in abilityKeys)
        {
            if (Input.GetKey(key))
            {
                return true;
            }
        }
        return false;
    }
    private bool PressedActionButtonUp()
    {
        foreach (var key in abilityKeys)
        {
            if (Input.GetKeyUp(key))
            {
                return true;
            }
        }
        return false;
    }
    public virtual void Update()
    {
        if (IsLocked()) return;
        if (OnCooldown()) return;
        if (PressedActionButtonDown())
        {
            AbilityKeyHeld();
        }
        if (PressedActionButtonUp())
        {
            AbilityKeyUp();
        }
    }
    public abstract void AbilityKeyHeld();
    public abstract void AbilityKeyUp();
    protected virtual void ExhaustAbility()
    {
        canUseAbility = false;
        SetCooldown();
    }
    private void SetCooldown()
    {
        StartCoroutine(SetCooldown_cr());
    }
    public virtual bool CanUseAbility()
    {
        if (onCooldown) return false;
        if (IsLocked()) return false;
        if (!canUseAbility) return false;
        return true;
    }
    public bool OnCooldown()
    {
        return onCooldown;
    }
    public List<KeyCode> GetAbilityKeys()
    {
        return abilityKeys;
    }
    private IEnumerator SetCooldown_cr()
    {
        abilityIcon.OnCooldown(abilityCooldown);
        onCooldown = true;
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
        abilityIcon.OffCooldown();
    }
}
