using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

/// <summary>
/// if they notice you you die, level restart:
/// 
/// if they notice you: 
///     brief animation and sound
///         optionally death screen
///     reload scene
/// </summary>
public abstract class Enemy : CollidableObject
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool startFacingRight, isStatic;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private GameObject noticedPopup;
    protected bool isDetector = false;
    public FieldOfView fov;
    protected Animator animator;
    protected float health;
    protected Action OnDamageTaken;
    protected bool noticedPlayer = false;
    [HideInInspector]
    public bool isFacingRight = true;
    private PlayerMovement player;
    protected Action OnNoticedPlayer;
    private const string KILL_PLAYER = "KillPlayer";


    protected virtual void Start()
    {
        if (startFacingRight) //inverse is set since the beginning of the coroutine swaps it
            isFacingRight = false;
        else
            isFacingRight = true;
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        health = maxHealth;
        OnDamageTaken += DamageTaken;
        OnNoticedPlayer += NoticedEventHelperMethod;
    }


    protected virtual void Update()
    {
        if (IsNoticed())
        {
            NoticedPlayer();
        }
    }
    private bool IsNoticed()
    {
        if (NoticeCondition()) 
            return true;
        if (fov != null && fov.IsPlayerInView() && IsEnemyFacingTowardsPlayer() && !noticedPlayer)
            return true;
        if (Input.GetKeyDown(KeyCode.V)) //TODO: remove, is a test
            return true;
        return false;
    }
    protected abstract bool NoticeCondition();
    private void NoticedEventHelperMethod()
    {
        noticedPlayer = true;
        StopAllCoroutines();
        noticedPopup.SetActive(true);
        if(!isDetector)
            HandleAnimatorOnNoticed();
        IncreaseMoveSpeed();
        FindObjectOfType<DeathManager>().Invoke(KILL_PLAYER, 1f);
    }
    private void IncreaseMoveSpeed()
    {
        moveSpeed *= 2.5f;
    }
    private void HandleAnimatorOnNoticed()
    {
        animator.SetBool("isRunning", true);
        animator.SetInteger("Horizontal", 0);
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
        OnNoticedPlayer?.Invoke();
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
            if(!isDetector)
                NoticedPlayer(); //immediately alerted if can be noticed by touch
        }
    }

    public override void TriggerEnter(Collider2D collision)
    {

    }
    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        health -= damage;
        //TODO: add blink and or freeze frame when an enemy takes damage
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
        print("ENEMY " + gameObject.name + " DIED.");
        //TODO: add particles and such
        Destroy(gameObject);
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool b, Vector3 p, float d, float a)
        {
            hit = b;
            point = p; 
            dist = d;
            angle = a;
        }
    }
}
