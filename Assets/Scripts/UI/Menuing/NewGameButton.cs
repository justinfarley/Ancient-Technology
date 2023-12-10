using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onClicked;
    private State state = State.Idle;
    public enum State
    {
        Idle,
        AreYouSure,
    }
    void Start()
    {
        onClicked.AddListener(ClickEvents);
    }

    void Update()
    {
        
    }
    private void ClickEvents()
    {
        if (!SaveSystem.HasData() || GameManager.instance.bestLevel <= 1)
        {
            StartNewGame();
        }
        else
        {
            if (state == State.Idle)
            {
                state = State.AreYouSure;
                TMP_Text text = GetComponentInChildren<TMP_Text>();
                text.text = "Are you sure?";
            }
            else if (state == State.AreYouSure)
            {
                StartNewGame();
            }
        }
    }
    private void StartNewGame()
    {
        if (SaveSystem.HasData())
            GameManager.DeleteData();
        GameManager.SaveGame();
        GameManager.LoadGame();
        SceneManager.LoadScene(1);//load introduction
    }
    public void OnClicked()
    {
        onClicked?.Invoke();
    }
}
