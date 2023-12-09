using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolEnemy : Enemy
{
    [Header("Start and End points for the patrol path")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [Header("Patrol Enemy Properties")]
    [SerializeField] private float downTimeBetweenMovements, timeToMove, maxMoveSpeed;
    private Vector2 startPos, endPos;
    private SpriteRenderer spriteRenderer;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = startTransform.position;
        endPos = endTransform.position;
        DialogueReader.Pair script1 = DialogueReader.GetScript(0);
        foreach(var s in script1.GetY())
        {
            print(s);
        }
        OnNoticedPlayer += () =>
        {
            //kill player;
        };
        Patrol(0);
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

    private void Patrol(float waitTime)
    {
        StartCoroutine(Patrol_cr(startPos, endPos, waitTime));
    }
    private IEnumerator Patrol_cr(Vector2 start, Vector2 end, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
        animator.SetInteger("Horizontal", 1);
        for(float f = 0; f < timeToMove; f += (Time.deltaTime * moveSpeed))
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
        StartCoroutine(Patrol_cr(end, start, 0));
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
