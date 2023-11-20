using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CollidableObject
{
    [SerializeField] protected float maxHealth;
    protected float health;
    protected Action OnDamageTaken;
    private void Start()
    {
        health = maxHealth;
        OnDamageTaken += DamageTaken;
    }
    public override void CollisionEnter(Collision2D collision)
    {
    }

    public override void TriggerEnter(Collider2D collision)
    {
    }
    protected virtual void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        health -= damage;
        if (health <= 0)
        {
            Die();
            OnDamageTaken?.Invoke();
            return;
        }
        OnDamageTaken?.Invoke();
    }
    private void DamageTaken()
    {
        print("Took damage");
    }
    protected virtual void Die()
    {

    }
}
