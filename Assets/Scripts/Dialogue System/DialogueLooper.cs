using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;
using Unity.VisualScripting;

public class DialogueLooper : MonoBehaviour
{
    protected TMP_Text dialogueText;
    public bool isDoneDialogue = false;
    [SerializeField] private float timeBetweenLetters, timeBetweenWords, timeBetweenSentences, speedMultiplier, timeBetweenScripts;
    public List<int> scriptsToRead;
    /// <summary>
    /// The respective action will get invoked when the key script is done running
    /// </summary>
    public static Dictionary<int, Action> scriptToActionMappings = new Dictionary<int, Action>();
    public void TriggerDialogue(Dictionary<int, Func<bool>> actionToWaitForAfterScript, bool leaveText)
    {
        if (scriptsToRead.Count <= 0) return;
        DialogueReader.Pair dialogueParts = DialogueReader.GetScript(scriptsToRead[0]);
        StartCoroutine(TriggerDialogue_cr(dialogueParts, actionToWaitForAfterScript,leaveText));
    }
    private void AddText(StringBuilder builder, string text)
    {
        builder.Append(text);
        dialogueText.text = builder.ToString();
    }
    private void ResetText(ref StringBuilder builder)
    {
        builder.Clear();
        dialogueText.text = "";
    }
    private IEnumerator WordRecur(StringBuilder builder, string word)
    {
        foreach(char c in word)
        {
            AddText(builder, c + "");
            yield return new WaitForSeconds(timeBetweenLetters * speedMultiplier);
        }
    }
    private IEnumerator SentenceRecur(StringBuilder builder, string sentence)
    {
        sentence.Replace(" ", "# ");
        string[] words = sentence.Split("#");
        foreach (string word in words)
        {
            yield return StartCoroutine(WordRecur(builder, word));
            yield return new WaitForSeconds(timeBetweenWords * speedMultiplier);
        }
    }
    private IEnumerator TriggerDialogue_cr(DialogueReader.Pair pair, Dictionary<int, Func<bool>> actionToWaitForAfterScript, bool leaveText)
    {
        StringBuilder builder = new StringBuilder();
        ResetText(ref builder);
        isDoneDialogue = false;
        foreach (string sentence in pair.GetY())
        {
            yield return StartCoroutine(SentenceRecur(builder, sentence));
            print(pair);
            if(Array.IndexOf(pair.GetY(), sentence) == pair.GetY().Length - 1) 
                AddText(builder, pair.GetX() + "");
            yield return new WaitForSeconds(timeBetweenSentences * speedMultiplier);
            builder.Clear();
        }
        if (scriptsToRead.Count > 0)
        {
            foreach (int v in DialogueReader.GetScripts().Keys){
                if (pair.GetIndex() == v)
                {
                    if (scriptToActionMappings.ContainsKey(v))
                    {
                        scriptToActionMappings[v]?.Invoke();
                    }
                }
            }
            print(scriptsToRead.Count);
            for (int i = 1; i < scriptsToRead.Count; i++)
            {
                int v = scriptsToRead[i - 1];
                int s = scriptsToRead[i];
                scriptsToRead.Remove(v);
                print(scriptsToRead.Count);
                if (actionToWaitForAfterScript != null && actionToWaitForAfterScript.ContainsKey(v))
                {
                    yield return new WaitUntil(actionToWaitForAfterScript[v]);
                }
                else
                {
                    yield return new WaitForSeconds(timeBetweenScripts);
                }
                yield return StartCoroutine(TriggerDialogue_cr(DialogueReader.GetScripts()[s], actionToWaitForAfterScript, leaveText));
            }
            print(scriptsToRead.Count);
            if (scriptsToRead.Count > 0)
            {
                int val = scriptsToRead[0];
                if (actionToWaitForAfterScript != null && actionToWaitForAfterScript.ContainsKey(val))
                {
                    yield return new WaitUntil(actionToWaitForAfterScript[val]);
                }
                else
                {
                    yield return new WaitForSeconds(timeBetweenScripts);
                }
                scriptsToRead.Clear();
            }
            if(!leaveText)
                dialogueText.text = "";
        }
        foreach (int v in DialogueReader.GetScripts().Keys)
        {
            if (pair.Equals(DialogueReader.GetScripts()[v]))
            {
                if (scriptToActionMappings.ContainsKey(v))
                {
                    scriptToActionMappings[v]?.Invoke();
                }
            }
        }
        isDoneDialogue = true;
    }
}
