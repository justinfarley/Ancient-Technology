using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onClicked;

    private void Start()
    {
        onClicked.AddListener(ClickEvents);
        if (!SaveSystem.HasData() || GameManager.instance.bestLevel <= 1) GetComponent<Button>().interactable = false;
    }
    private void ClickEvents()
    {
        print(GameManager.instance.bestLevel);
        SceneManager.LoadScene(GameManager.instance.bestLevel);
    }
    public void OnClicked()
    {
        onClicked?.Invoke();
    }
}
