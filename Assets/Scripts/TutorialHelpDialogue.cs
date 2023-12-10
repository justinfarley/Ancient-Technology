using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TutorialHelpDialogue : DialogueLooper
{
    Dictionary<int, Func<bool>> keyValuePairs = new Dictionary<int, Func<bool>>();
    private void Start()
    {
        keyValuePairs.Add(5, HasMoved);
        keyValuePairs.Add(6, HasJumped);
        dialogueText = GetComponent<TMP_Text>();
        TriggerDialogue(keyValuePairs, false);
    }
    private bool HasMoved()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }
    private bool HasJumped()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
