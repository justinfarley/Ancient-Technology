using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class CollidableObject : MonoBehaviour
{
    [Space(10f)]
    [Header("CollidableObject Fields")]
    protected Collider2D _col;
    protected Rigidbody2D _rb;
    [SerializeField] private bool useTrigger;
    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        if(useTrigger && !_col.isTrigger) _col.isTrigger = true;
        if (!useTrigger && _col.isTrigger) _col.isTrigger = false;
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!useTrigger) return;
        TriggerEnter(collision);
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger) return;
        CollisionEnter(collision);
    }
    public abstract void CollisionEnter(Collision2D collision);
    public abstract void TriggerEnter(Collider2D collision);

}
