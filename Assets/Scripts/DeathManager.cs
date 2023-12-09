using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private Image background;
    public void KillPlayer()
    {
        Time.timeScale = 1;
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        foreach(var v in player.GetComponents<Ability>()) //disable all abilities
        {
            v.enabled = false;
        }
        player.enabled = false;
        
        StartCoroutine(FadeToRed_cr(background, 1));
    }
    private IEnumerator FadeToRed_cr(Image image, float timeToFade)
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        for (float i = 0; i < timeToFade; i += Time.deltaTime)
        {
            player.GetComponent<RecoilShake>().ScreenShake(new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));
            Color color = image.color;
            color.a = i / timeToFade;
            image.color = color;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
