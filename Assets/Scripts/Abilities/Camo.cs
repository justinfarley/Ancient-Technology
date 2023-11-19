using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camo : Ability
{
    private float timeToUseAbility = 2f;
    private float chargeTime = 0f;
    private bool isInvisible = false;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = abilityIcon.GetAbilityOverlayImage().color;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void AbilityKeyHeld()
    {
        if (isInvisible) return;
        chargeTime += Time.deltaTime;
        Color orange = new Color32(255,165,0,255);
        orange.a = 0.5f;
        abilityIcon.GetAbilityOverlayImage().color = orange;
        abilityIcon.GetAbilityOverlayImage().fillAmount = chargeTime / timeToUseAbility;
        if(chargeTime >= timeToUseAbility)
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
        UseAbility();
    }
    public override void AbilityKeyUp()
    {
        BecomeVisible();
    }
    public override bool CanUseAbility()
    {
        return base.CanUseAbility();
    }
    protected override void UseAbility()
    {
        base.UseAbility();
        isInvisible = false;
        chargeTime = 0f;
        abilityIcon.GetAbilityOverlayImage().color = originalColor;
        abilityIcon.GetAbilityOverlayImage().fillAmount = 0f;
    }
    public bool UsingAbility()
    {
        if (isInvisible) return true;
        if (chargeTime > 0) return true;
        return false;
    }
}
