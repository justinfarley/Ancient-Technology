using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractAbilityIcon : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    protected Ability ability;
    protected Image abilityIconImage, abilityOverlayImage;
    private Color disabledColor = new Color32(255, 127, 127, 255);

    protected virtual void Awake()
    {
        abilityIconImage = GetComponent<Image>();
        abilityOverlayImage = transform.GetChild(0).GetComponent<Image>();
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
    
}
