using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camo : Ability
{
    private float timeToUseAbility = 2f;
    private float chargeTime = 0f;
    private bool isInvisible = false;

    public override void Awake()
    {
        base.Awake();
        OnUnlock += () =>
        {
            type = Type.Both;
            locked = false;
            GameManager.instance.HasCamo = true;
        };

    }
    public override void Start()
    {
        base.Start();
        if (GameManager.instance.HasCamo)
        {
            Unlock();
        }
    }
    public override void Update()
    {
        base.Update();
    }
    public override void AbilityKeyHeld()
    {
        if (isInvisible)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            return;
        }
        chargeTime += Time.deltaTime;
        abilityIcon.abilityOverlayImage.fillAmount = chargeTime / timeToUseAbility;
        if (!GetComponent<PlayerMovement>().IsGrounded())
        {
            chargeTime = 0f;
            abilityIcon.abilityOverlayImage.fillAmount = chargeTime / timeToUseAbility;
        }
        if (chargeTime >= timeToUseAbility)
        {
            BecomeInvisible();
        }
    }
    private void BecomeInvisible()
    {
        if(!isInvisible)
            GetComponent<PlayerMovement>().TurnedCamo();
        chargeTime = 0f;
        isInvisible = true;
        Color c = spriteRenderer.color;
        c.a = 0.5f;
        spriteRenderer.color = c;
    }
    private void BecomeVisible()
    {
        if (isInvisible)
            GetComponent<PlayerMovement>().TurnedVisible();
        chargeTime = 0f;
        isInvisible = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        ExhaustAbility();
    }
    public override void AbilityKeyUp()
    {
        base.AbilityKeyUp();
        BecomeVisible();
    }
    public override void AbilityKeyDown()
    {
        base.AbilityKeyDown();
    }
    public override bool CanUseAbility()
    {
        return base.CanUseAbility();
    }
    public override void ExhaustAbility()
    {
        base.ExhaustAbility();
        isInvisible = false;
        chargeTime = 0f;
    }
    public bool UsingAbility()
    {
        if (isInvisible) return true;
        if (chargeTime > 0) return true;
        return false;
    }
    public bool IsInvisible()
    {
        return isInvisible;
    }
}
