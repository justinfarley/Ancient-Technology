using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Roger_NPC : DialogueLooper
{
    public enum TriggerType
    {
        Awake,
        OnTriggerEnter,
        OnTriggerExit,
        Script,
    }
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private GameObject gauntlet;
    [SerializeField] private GameObject player;
    [Header("Defines Which Script to read")]
    private bool wasTriggered = false;
    void Start()
    {
        dialogueText = GetComponentInChildren<TMP_Text>();
        dialogueText.text = "";
        //TriggerDialogue(0, '.');
        scriptToActionMappings.Add(2, () =>
        {
            if (FindObjectOfType<PlayerUnlockManager>().GetTeleport().IsLocked())
                PlayerUnlockManager.UnlockAbility(FindObjectOfType<PlayerUnlockManager>().GetTeleport());
        });

    }

    void Update()
    {
        LookAtPlayer();
        if(wasTriggered && isDoneDialogue)
        {
            print("done dialogue");
        }
    }
    //
    public void UnlockCorrespondingAbility()
    {
        //if (FindObjectOfType<PlayerUnlockManager>().GetTeleport().IsLocked())
        //    PlayerUnlockManager.UnlockAbility(FindObjectOfType<PlayerUnlockManager>().GetTeleport());
        if (FindObjectOfType<PlayerUnlockManager>().GetCamo().IsLocked())
            PlayerUnlockManager.UnlockAbility(FindObjectOfType<PlayerUnlockManager>().GetCamo());
        if (FindObjectOfType<PlayerUnlockManager>().GetTimeSlow().IsLocked())
            PlayerUnlockManager.UnlockAbility(FindObjectOfType<PlayerUnlockManager>().GetTimeSlow());
    }
    private void LookAtPlayer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //flipx = true means looking left
        if(player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            if(triggerType == TriggerType.OnTriggerEnter)
            {
                TriggerDialogue();
            }
        }
    }
    public new void TriggerDialogue()
    {
        if (wasTriggered) return;
        base.TriggerDialogue();
        wasTriggered = true;
    }
    public void TriggerBeginningDialogue()
    {
        TriggerDialogue();
    }
    public void SpawnGauntlet()
    {
        Instantiate(gauntlet, transform.position, Quaternion.identity);
    }
}
