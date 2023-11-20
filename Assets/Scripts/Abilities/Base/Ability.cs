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
    protected bool activated  = false;
    [SerializeField] private List<KeyCode> abilityKeys;
    private GameObject abilityUIIcon;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected AbstractAbilityIcon abilityIcon;
    [SerializeField] protected Color eyeColor;
    protected Color lastColor;
    protected static List<Color> colorList = new List<Color>();

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (!activated && PressedActionButtonDown())
        {
            OnActivation();
            activated = true;
        }
        else if (PressedActionButtonDown())
        {
            AbilityKeyHeld();
        }
        if (PressedActionButtonUp())
        {
            AbilityKeyUp();
        }
    }
    public virtual void OnActivation()
    {
        SetToEyeColor();
    }
    public abstract void AbilityKeyHeld();
    public abstract void AbilityKeyUp();
    protected virtual void ExhaustAbility()
    {
        canUseAbility = false;
        activated = false;
        ResetToLastColor(eyeColor);
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
    protected void ResetToLastColor(Color colorToRemove)
    {
        foreach(var v in colorList)
        {
            print(v);
        }
        if (colorList.Count > 1)
        {
            colorList.Remove(colorToRemove);
            spriteRenderer.color = colorList[colorList.Count - 1];
        }
        else
            ResetToWhite();
    }
    protected void ResetToWhite()
    {
        colorList.Clear();
        spriteRenderer.color = Color.white;
    }
    protected void SetToEyeColor()
    {
        colorList.Add(eyeColor);
        spriteRenderer.color = colorList[colorList.Count - 1];
    }
}
