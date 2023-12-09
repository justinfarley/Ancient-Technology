using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractAbilityIcon : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    protected Ability ability;
    protected Image abilityIconImage, abilityOverlayImage;
    private Color disabledColor = new Color32(255, 127, 127, 255);
    private float iconSize = 150f;

    protected virtual void Awake()
    {
        abilityIconImage = transform.GetChild(0).GetComponent<Image>();
        abilityOverlayImage = transform.GetChild(1).GetComponent<Image>();
        print(transform.GetChild(1).GetComponent<Image>().name);
    }
    protected virtual void Start()
    {
        abilityOverlayImage.fillAmount = 0;
    }
    //public Image GetAbilityIconImage() { return abilityIcon; }
    public Image GetAbilityOverlayImage() { return abilityOverlayImage; }
    public void OffCooldown()
    {
        abilityIconImage.color = Color.white;
        abilityOverlayImage.fillAmount = 0;
    }
    public void OnCooldown(float t)
    {
        abilityIconImage.color = disabledColor;
        StartCoroutine(OnCooldown_cr(t));
    }
    private IEnumerator OnCooldown_cr(float t)  
    {
        for(float f = 0; f < t; f += Time.deltaTime)
        {
            abilityOverlayImage.fillAmount = f / t;
            yield return null;
        }
    }
    public void UnlockAnimationEvent()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<RectTransform>().sizeDelta = new Vector2(iconSize, iconSize);
        transform.localScale = new Vector3(1, 1, 1);
        
    }

}
