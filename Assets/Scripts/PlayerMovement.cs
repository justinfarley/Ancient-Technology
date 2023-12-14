using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PlayerMovement : CollidableObject
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float speed, groundCheckRadius, maxHSpeed, maxVSpeed;
    [SerializeField] private LayerMask groundCheckIgnoreLayers;
    [SerializeField] private Vector2 jumpForce;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float x;
    private Vector2 moveForce;
    private float jumpDelay = 0f;
    public bool AbilitiesDisabled { get; set; }
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        HandleMovement();
        _rb.AddForce(moveForce, ForceMode2D.Impulse);
        _rb.velocity = GetClampedMoveVector(_rb.velocity, -maxHSpeed, maxHSpeed, -maxVSpeed, maxVSpeed);
    }
    private static Vector2 GetClampedMoveVector(Vector2 vector, float minH, float maxH, float minV, float maxV)
    {
        float x = Mathf.Clamp(vector.x, minH, maxH);
        float y = Mathf.Clamp(vector.y, minV, maxV);
        return new Vector2(x, y);
    }   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            HandleJump();
        }
        HandleSpriteFlipping();
        if (AbilitiesDisabled)
        {
            AbilitiesDisabled = false;
            foreach(var v in GetComponents<Ability>())
            {
                v.enabled = false;
            }
            Time.timeScale = 1;
        }
    }
    private void HandleMovement()
    {
        x = Input.GetAxisRaw("Horizontal");

        animator.SetInteger("Horizontal", (int)x);

        moveForce = new Vector2(x, moveForce.y);
    }
    private void HandleJump()
    {
        if (!IsGrounded()) return;
        if (GetComponent<Camo>() && GetComponent<Camo>().UsingAbility()) return; //if using camo, cannot jump
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
    }
    private void HandleSpriteFlipping()
    {
        //this works for us since the idle animation is 
        float xVal = animator.GetInteger("Horizontal");
        if (xVal == 0) return;
        if (xVal < 0) //if moving left
        {
            LookLeft();
        }
        else LookRight();
    }
    private void LookLeft()
    {
        spriteRenderer.flipX = true; //maybe change to rotating 180 on the Y axis
    }
    private void LookRight()
    {
        spriteRenderer.flipX = false;
    }
    public bool IsMoving()
    { 
        //TODO: this next
        return false;
    }
    public bool IsGrounded()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, ~groundCheckIgnoreLayers);
        foreach (Collider2D col in hits)
        {
            if (col.gameObject.GetComponent<Ground>()) return true;
        }
        return false;
    }
    private void Jump()
    {
        if (jumpDelay > 0) return;
        _rb.velocity = new Vector2(_rb.velocity.x, 0); //remove for conservation of vertical momentum
        _rb.AddForce(jumpForce, ForceMode2D.Impulse);
        jumpDelay = 0.1f;
        StartCoroutine(JumpDelay_cr());
    }
    private IEnumerator JumpDelay_cr()
    {
        for(float i = jumpDelay; i > 0; i -= Time.deltaTime)
        {
            jumpDelay = i;
            yield return null;
        }
        jumpDelay = 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    public void TurnedCamo()
    {
        maxHSpeed /= 2;
        maxVSpeed /= 2;
    }
    public void TurnedVisible()
    {
        maxVSpeed *= 2;
        maxHSpeed *= 2;
    }
    public override void CollisionEnter(Collision2D collision)
    {
    }

    public override void TriggerEnter(Collider2D collision)
    {
    }
    public void SetMaxHSpeed(float speed)
    {
        maxHSpeed += speed;
    }
    public void SetMaxVSpeed(float speed)
    {
        maxVSpeed += speed;
    }
    public float GetMaxVSpeed()
    {
        return maxVSpeed;
    }
    public float GetMaxHSpeed()
    {
        return maxHSpeed;
    }
}
