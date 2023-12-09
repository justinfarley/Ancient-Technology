using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGauntlet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float force = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-0.2f, 1) * force, ForceMode2D.Impulse);
    }
}
