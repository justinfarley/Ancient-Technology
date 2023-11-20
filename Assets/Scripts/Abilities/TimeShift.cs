using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeShift : Ability
{
    //TODO: Implementation
    /**
     * When key is pressed, the ability is activated for X REAL seconds (starting at 10)
     * while the ability is active, everything will move at 0.5f time scale 
     * (maybe including the player we will see probably not)
     * during this X REAL seconds interval the player can use the left and right arrow keys to shift the time scale
     * up or down (left = -0.1f time scale and right = 0.1f time scale)
     * 
     * then after the seconds are over simply put it on cooldown and set time to the original 1f
     */
    private bool activated = false;
    private bool slowedTime = false;
    [SerializeField] private TMP_Text timerText, timeScaleText;
    [SerializeField] private KeyCode increaseTimeScaleKey, decreaseTimeScaleKey, maxTimeScaleKey, minTimeScaleKey;
    private readonly float timeIncrement = 0.1f, minTimeScale = 0.1f, maxTimeScale = 2f, abilityDuration = 10f;
    private Color ogColor;
    private Image abilityIconOverlayImg;
    public override void Start()
    {
        base.Start();
        abilityIconOverlayImg = abilityIcon.transform.GetChild(0).GetComponent<Image>();
        ogColor = abilityIconOverlayImg.color;
    }
    public override void Update()
    {
        base.Update();
        if (!slowedTime) return;
        if (!activated)
        {
            OnActivation();
        }
        if (Input.GetKeyDown(increaseTimeScaleKey) || Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            IncreaseTime(timeIncrement);
        }
        else if (Input.GetKeyDown(maxTimeScaleKey))
        {
            IncreaseTime(maxTimeScale);
        }
        if (Input.GetKeyDown(decreaseTimeScaleKey) || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            DecreaseTime(timeIncrement);
        }
        else if (Input.GetKeyDown(minTimeScaleKey))
        {
            DecreaseTime(Time.timeScale);
        }
        timeScaleText.text = Time.timeScale.ToString("F1") + "x";
    }
    private void OnActivation()
    {
        StartCoroutine(IncreaseTime_cr());
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        timeScaleText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        print("gel");
        Color yellow = Color.yellow;
        yellow.a = 0.5f;
        abilityIconOverlayImg.color = yellow;
        abilityIconOverlayImg.fillAmount = 1;
        activated = true;
    }
    private void IncreaseTime(float amount)
    {
        IncrementTimeScale(amount);
    }
    private void DecreaseTime(float amount)
    {
        IncrementTimeScale(-amount);
    }
    private void IncrementTimeScale(float amount)
    {
        Time.timeScale += amount;
        if (Time.timeScale < 0)
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        else
            Time.fixedDeltaTime = 0.02f;
        Time.timeScale = Mathf.Clamp(Time.timeScale, minTimeScale, maxTimeScale);
    }
    protected override void ExhaustAbility()
    {
        slowedTime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        timerText.gameObject.SetActive(false);
        timeScaleText.gameObject.SetActive(false);
        abilityIconOverlayImg.color = ogColor;
        abilityIconOverlayImg.fillAmount = 1;
        activated = false;
        base.ExhaustAbility();
    }
    public override void AbilityKeyHeld()
    {
        if(!slowedTime)
        {
            slowedTime = true;
        }
    }
    public override void AbilityKeyUp()
    {
        //UNCOMMENT IF YOU WANT ABILITY TO END IF USER LETS GO OF THE CTRL KEY AND NOT ONLY WHEN TIMER RUNS OUT
    /*      if (!slowedTime)
            {
                slowedTime = false;
            }
            UseAbility();
            timerText.gameObject.SetActive(true);
            StopAllCoroutines();*/
    }
    public override bool CanUseAbility()
    {
        if(!base.CanUseAbility()) return false;
        if (slowedTime) return false;
        return true;
    }
    private IEnumerator IncreaseTime_cr()
    {
        //use Time.unscaledDeltaTime for events that happen during this (aka the countdown on the icon)
        slowedTime = true;
        for (float f = abilityDuration; f > 0; f -= Time.unscaledDeltaTime)
        {
            timerText.text = string.Format("{0:0.0}",f);
            yield return null;
        }
        ExhaustAbility();
    }

}
