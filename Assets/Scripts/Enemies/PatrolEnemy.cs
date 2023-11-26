using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolEnemy : Enemy
{
    [Header("Start and End points for the patrol path")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [Header("Patrol Enemy Properties")]
    [SerializeField] private float downTimeBetweenMovements, timeToMove, moveSpeed, maxMoveSpeed;
    private Vector2 startPos, endPos;
    private SpriteRenderer spriteRenderer;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = startTransform.position;
        endPos = endTransform.position;

        Patrol();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void FixedUpdate()
    {
        ClampVelocity();
    }
    private void ClampVelocity()
    {
        Vector2 moveVector = _rb.velocity;
        moveVector.x = Mathf.Clamp(moveVector.x, -maxMoveSpeed, maxMoveSpeed);
        _rb.velocity = moveVector;
    }

    private void Patrol()
    {
        StartCoroutine(Patrol_cr(startPos, endPos));
    }
    private IEnumerator Patrol_cr(Vector2 start, Vector2 end)
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
        animator.SetInteger("Horizontal", 1);
        for(float f = 0; f < timeToMove; f += Time.deltaTime)
        {
            float elapsed = f / timeToMove;
            _rb.MovePosition(Vector2.Lerp(start, end, elapsed));
            //_rb.AddForce(speed * Time.fixedDeltaTime * dir, ForceMode2D.Impulse);
            yield return null;
            if (Vector2.Distance(transform.position, end) <= 0.001f)
            {
                break;
            }
        }
        animator.SetInteger("Horizontal", 0);
        yield return new WaitForSeconds(downTimeBetweenMovements);
        StartCoroutine(Patrol_cr(end, start));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPos, 0.2f);
        Gizmos.DrawWireSphere(endPos, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
