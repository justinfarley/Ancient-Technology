using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{
    private bool atLadder = false;
    private PlayerMovement player;
    [SerializeField] private Transform startPos, endPos;
    [SerializeField] private float climbTime;
    [SerializeField] private Image blackScreen;
    [SerializeField] private float speed;
    private const string CLIMB_LADDER = "ClimbLadder";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            atLadder = true;
            player = collision.gameObject.GetComponent<PlayerMovement>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            atLadder = false;
        }
    }
    private void Update()
    {
        if(atLadder && Input.GetKeyDown(KeyCode.W) && player.IsGrounded())
        {
            atLadder = false;
            //trigger animation
            player.GetComponent<Animator>().SetTrigger(CLIMB_LADDER);
            ClimbLadderUp();
        }

    }
    private void ClimbLadderUp()
    {
        GameManager.SaveGame();
        player.GetComponent<Ability>().ResetToWhite(); //maybe event for this sometime soon
        player.AbilitiesDisabled = true;
        StartCoroutine(ClimbLadder_cr(true));
    }
    private IEnumerator ClimbLadder_cr(bool start)
    {
        //first the player to the
        if (start)
        {
            if (player.transform.position.x > transform.position.x)
            {
                if (!player.GetComponent<SpriteRenderer>().flipX)
                {
                    player.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else if (player.transform.position.x <= transform.position.x)
            {
                if (player.GetComponent<SpriteRenderer>().flipX)
                {
                    player.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            player.GetComponent<Animator>().Play("whiteeyes_running", 0);
            Vector2 playerStart = player.transform.position;
            for (float i = 0; i < 0.25f; i += Time.deltaTime)
            {
                float elapsed = i / 0.25f;
                player.transform.position = Vector2.Lerp(playerStart, new Vector2(startPos.position.x, player.transform.position.y), elapsed);
                yield return null;
            }
        }
        player.transform.position = new Vector2(startPos.position.x, player.transform.position.y);
        startPos.position = player.transform.position;
        for (float i = 0; i < climbTime; i += Time.deltaTime)
        {
            float elapsed = i / climbTime;
            player.transform.position = Vector2.Lerp(startPos.position, endPos.position, elapsed);
            yield return null;
        }
        StartCoroutine(FadeToBlack_cr(blackScreen, 3));
        startPos.position = player.transform.position;
        endPos.position = player.transform.position + new Vector3(0f, 9f);
        yield return StartCoroutine(ClimbLadder_cr(false));
    }
    private IEnumerator FadeToBlack_cr(Image image, float fadeTime)
    {
        for(float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            float opacity = i / fadeTime;
            Color c = image.color;
            c.a = opacity;
            image.color = c;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
