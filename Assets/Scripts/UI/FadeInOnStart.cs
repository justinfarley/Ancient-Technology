using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOnStart : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float timeToFade;
    private Image image;

    private void Start()    
    {
        image = GetComponent<Image>();
        FadeIn();
    }
    private void FadeIn()
    {
        StartCoroutine(FadeIn_cr());
    }
    private IEnumerator FadeIn_cr()
    {
        for(float i = timeToFade; i > 0; i -= Time.deltaTime)
        {
            Color c = image.color;
            c.a = i / timeToFade;
            image.color = c;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}
