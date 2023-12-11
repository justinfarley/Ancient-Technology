using System;
using System.Collections;
using System.Collections.Generic;
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
        OnScreen,
    }
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private GameObject gauntlet;
    [SerializeField] private GameObject player;
    private bool wasTriggered = false;
    private bool leaveDialogueText = true;
    void Start()
    {
        scriptToActionMappings = new Dictionary<int, Action>();
        dialogueText = GetComponentInChildren<TMP_Text>();
        dialogueText.text = "";
        //TriggerDialogue(0, '.');
        AddScriptMapping(2, () =>
        {
            if (FindObjectOfType<PlayerUnlockManager>().GetTeleport().IsLocked())
                PlayerUnlockManager.UnlockAbility(FindObjectOfType<PlayerUnlockManager>().GetTeleport());
        });
        AddScriptMapping(1, SpawnGauntlet);

    }

    void Update()
    {
        LookAtPlayer();
        if(wasTriggered && isDoneDialogue)
        {
            //print("done dialogue");
        }
    }
    private void AddScriptMapping(int index, Action a)
    {
        if(!scriptToActionMappings.ContainsKey(index))
        {
            scriptToActionMappings.Add(index, a);
        }
    }
    private void OnBecameVisible()
    {
        if(triggerType == TriggerType.OnScreen)
        TriggerDialogue(null, leaveDialogueText);
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
                TriggerDialogue(null, leaveDialogueText);
            }
        }
    }
    public new void TriggerDialogue(Dictionary<int, Func<bool>> waitUntilDict, bool leaveText)
    {
        if (wasTriggered) return;
        base.TriggerDialogue(waitUntilDict, leaveText);
        wasTriggered = true;
    }
    public void TriggerBeginningDialogue()
    {
        TriggerDialogue(null, leaveDialogueText);
    }
    public void SpawnGauntlet()
    {
        Instantiate(gauntlet, transform.position, Quaternion.identity);
    }
}
