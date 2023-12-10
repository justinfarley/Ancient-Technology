using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] protected UnityEvent OnWallEventTriggered;
    private bool isTriggered = false;
    private void Awake()
    {
        OnWallEventTriggered.AddListener(() =>
        {
            isTriggered = true;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.GetComponent<PlayerMovement>())
        {
            OnWallEventTriggered?.Invoke();
        }
    }
}
