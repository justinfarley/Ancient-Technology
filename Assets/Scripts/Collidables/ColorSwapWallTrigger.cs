using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwapWallTrigger : WallTrigger
{
    [SerializeField] private Color colorToSwapTo;

    private void Start()
    {
        //OnWallEventTriggered.AddListener(SwapColor);
    }
    public void SwapColor()
    {
        GetComponent<SpriteRenderer>().color = colorToSwapTo;

    }
}
