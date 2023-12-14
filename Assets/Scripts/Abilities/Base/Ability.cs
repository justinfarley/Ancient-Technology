using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : Unlockable
{
    public enum Abilities
    {
        None,
        Teleportation,
        Camo,
        TimeSlow,
        Attack,
    }
    [SerializeField]
    protected float abilityCooldown;
    protected bool canUseAbility = false;
    private bool onCooldown = false;
    protected bool activated  = false;
    [SerializeField] private List<KeyCode> abilityKeys;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected AbstractAbilityIcon abilityIcon;
    [SerializeField] protected Color eyeColor;
    protected Color ogColor;
    protected Color lastColor;
    protected static List<Color> colorList = new List<Color>();
    protected Type type;
    public enum Type
    {
        None,
        FillOnly,
        ColorOnly,
        Both
    }
    public override void Awake()
    {
        base.Awake();
        OnUnlock += () =>
        {
            abilityIcon.abilityIconImage.gameObject.SetActive(true);
            abilityIcon.GetComponent<Animator>().SetTrigger("Unlocked");
            print(abilityIcon.abilityIconImage.name);
        };
    }
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogColor = abilityIcon.abilityOverlayImage.color;
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
            AbilityKeyDown();
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
    public virtual void AbilityKeyDown()
    {
        SetToEyeColor();
        SetIconOverlayColor(eyeColor);
    }
    private void SetIconOverlayColor(Color color)
    {
        switch (type)
        {
            case Type.Both:
                Color temp = color;
                temp.a = 0.5f;
                abilityIcon.abilityOverlayImage.color = temp;
                abilityIcon.abilityOverlayImage.fillAmount = 1;
                break;
            case Type.ColorOnly:
                temp = color;
                temp.a = 0.5f;
                abilityIcon.abilityOverlayImage.color = temp;
                break;
            case Type.FillOnly:
                abilityIcon.abilityOverlayImage.fillAmount = 1;
                break;
            case Type.None:
                break;
        }

    }
    public virtual void AbilityKeyHeld()
    {
    }
    public virtual void AbilityKeyUp()
    {
        abilityIcon.abilityOverlayImage.color = ogColor;
        abilityIcon.abilityOverlayImage.fillAmount = 0;
    }
    public virtual void ExhaustAbility()
    {
        canUseAbility = false;
        activated = false;
        abilityIcon.abilityOverlayImage.color = ogColor;
        abilityIcon.abilityOverlayImage.fillAmount = 1;
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
        if (colorList.Count > 1)
        {
            colorList.Remove(colorToRemove);
            spriteRenderer.color = colorList[colorList.Count - 1];
        }
        else
            ResetToWhite();
    }
    public void ResetToWhite()
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
