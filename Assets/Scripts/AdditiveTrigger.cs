using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveTrigger : MonoBehaviour
{
    [SerializeField] private List<int> scriptsToAdd = new List<int>();
    [SerializeField] private DialogueLooper looperToAddTo;
    [SerializeField] private bool leaveText;
    private bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerMovement>() && !wasTriggered)
        {
            wasTriggered = true;
            foreach (var v in scriptsToAdd)
            {
                looperToAddTo.scriptsToRead.Add(v);
            }
            print(looperToAddTo.isDoneDialogue);
            try
            {
                if (!looperToAddTo.isDoneDialogue)
                    looperToAddTo.scriptsToRead.RemoveAt(0);
            }catch(Exception)
            {
                Debug.LogError("Make sure to toggle the \"isDoneDialogue\" option in the DialogueReader being modified");
            }
            looperToAddTo.StopAllCoroutines();
            looperToAddTo.TriggerDialogue(null, leaveText);
        }
    }


}
