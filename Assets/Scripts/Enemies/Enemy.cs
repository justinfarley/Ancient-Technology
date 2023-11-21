using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// if they notice you you die, level restart:
/// 
/// if they notice you: 
///     brief animation and sound
///         optionally death screen
///     reload scene
/// </summary>
public class Enemy : CollidableObject
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool startFacingRight;
    [SerializeField] protected float sightRange;
    [SerializeField] private GameObject noticedPopup;
    protected float health;
    protected Action OnDamageTaken;
    protected bool noticedPlayer = false, isFacingRight = true;
    private PlayerMovement player;


    protected virtual void Start()
    {
        if (startFacingRight) //inverse is set since the beginning of the coroutine swaps it
            isFacingRight = false;
        else
            isFacingRight = true;
        player = FindObjectOfType<PlayerMovement>();
        health = maxHealth;
        OnDamageTaken += DamageTaken;
    }
    protected virtual void Update()
    {
        if (IsPlayerInSightDistance() && IsEnemyFacingTowardsPlayer() && !noticedPlayer)
        {
            NoticedPlayer();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Instantiate(this.gameObject, gameObject.transform.position, Quaternion.identity); 
        }
    }
    private void NoticedPlayer()
    {
        Camo playerCamo = player.GetComponent<Camo>();
        if (playerCamo.IsInvisible())
        {
            print("NOTICED PLAYER! (but they are invisible...)");
            return;
        }
        //-----------------------------------------------------------------
        print("NOTICED PLAYER!");
        noticedPlayer = true;
        StopAllCoroutines();
        noticedPopup.SetActive(true);
        StartCoroutine(NewSpawn_cr());
        //Destroy(gameObject); //test
    }
    /// <summary>
    /// test method
    /// </summary>
    /// <returns></returns>
    private IEnumerator NewSpawn_cr()
    {
        yield return new WaitForSeconds(2);
        Enemy e =Instantiate(gameObject, gameObject.transform.position, Quaternion.identity).GetComponent<Enemy>();
        e.health = e.maxHealth;
        e.noticedPlayer = false;
        e.noticedPopup.SetActive(false);
        Destroy(gameObject);
    }

    private bool IsPlayerInSightDistance()
    {
        float xDistFromPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        float yDistFromPlayer = Mathf.Abs(player.transform.position.y - transform.position.y);
        Debug.DrawRay(gameObject.transform.position, new Vector2(xDistFromPlayer, 2*yDistFromPlayer / 3));
        return xDistFromPlayer <= sightRange && yDistFromPlayer <= 2*sightRange / 3;
    }
    private bool IsEnemyFacingTowardsPlayer()
    {
        if (isFacingRight && player.transform.position.x > transform.position.x)
        {
            return true;
        }
        else if (!isFacingRight && player.transform.position.x < transform.position.x)
        {
            return true;
        }
        return false;
    }
    public override void CollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) //if player collided with enemy
        {
            NoticedPlayer(); //immediately alerted
        }
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
