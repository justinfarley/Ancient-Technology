using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    [Header("Start and End points for the patrol path")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [Header("Enemy Properties")]
    [SerializeField] private float sightRange;
    [SerializeField] private float downTimeBetweenMovements, timeToMove, moveSpeed, maxMoveSpeed;
    [SerializeField] private bool startFacingRight;
    private bool noticedPlayer = false, isFacingRight = true;
    private PlayerMovement player;
    private Vector2 startPos, endPos;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = startTransform.position;
        endPos = endTransform.position;
        if (startFacingRight) //inverse is set since the beginning of the coroutine swaps it
            isFacingRight = false;
        else
            isFacingRight = true;
        Patrol();
    }
    private void Update()
    {
        if(IsPlayerInSightDistance() && IsEnemyFacingTowardsPlayer() && !noticedPlayer)
        {
            NoticedPlayer();
        }
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
    private bool IsPlayerInSightDistance()
    {
        return Vector2.Distance(player.transform.position, transform.position) <= sightRange;
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
    private void NoticedPlayer()
    {
        print("NOTICED PLAYER!");
        noticedPlayer = true;
        StopAllCoroutines();
        Destroy(gameObject); //test
    }
    private void Patrol()
    {
        StartCoroutine(Patrol_cr(startPos, endPos));
    }
    private IEnumerator Patrol_cr(Vector2 start, Vector2 end)
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = isFacingRight;
        Vector2 dir = end - start;
        float distance = Vector2.Distance(start, end);
        float time = timeToMove;
        float speed = distance / time;
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
