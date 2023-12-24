using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    [Header("Patrol Enemy Properties")]
    [SerializeField] private float downTimeBetweenMovements, waitTime, moveForTime;
    [SerializeField] private Sprite rightFacingSprite;
    [SerializeField] private bool runner;
    private SpriteRenderer spriteRenderer;
    private Vector2 graphicSphere1, graphicSphere2;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DialogueReader.Pair script1 = DialogueReader.GetScript(0);
        foreach(var s in script1.GetY())
        {
            print(s);
        }
        OnNoticedPlayer += () =>
        {
            //kill player;
        };
        if (!isStatic)
        {
            animator.SetBool("isRunning", runner);
            Patrol(waitTime);
        }
        else
        {
            if (!startFacingRight)
            {
                spriteRenderer.flipX = true;
                isFacingRight = false;
            }
        }
        
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
    protected override void Update()
    {
        base.Update();
    }
    private void FixedUpdate()
    {

    }
    private void SwapFovPos()
    {
        if (runner)
        {
            Vector2 pos = fov.transform.localPosition;
            pos.x = isFacingRight ? 0.2f : -0.2f;
            fov.transform.localPosition = pos;
        }
    }
    private void ResetFovPos()
    {
        if (runner)
        {
            Vector2 pos = fov.transform.localPosition;
            pos.x = 0;
            fov.transform.localPosition = pos;
        }
    }
    private void Patrol(float waitTime)
    {
        StartCoroutine(Patrol_cr(waitTime));
    }
    private IEnumerator Patrol_cr(float waitTime)
    {
        if (waitTime < 0) yield break;
        yield return new WaitForSeconds(waitTime);
        animator.enabled = true;
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
        SwapFovPos();
        if(!isStatic)
            animator.SetInteger("Horizontal", 1);
        else GetComponent<SpriteRenderer>().sprite = rightFacingSprite;
        for (float i = 0; i <= moveForTime; i += Time.deltaTime)
        {
            _rb.velocity = new Vector2(isFacingRight ? moveSpeed : -moveSpeed, _rb.velocity.y);
            yield return null;
        }
        _rb.velocity = new Vector2(isFacingRight ? moveSpeed : -moveSpeed, _rb.velocity.y);
        yield return null;
        _rb.velocity = Vector2.zero;
        if (!isStatic)
            animator.SetInteger("Horizontal", 0);
        else GetComponent<SpriteRenderer>().sprite = rightFacingSprite;
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = rightFacingSprite;
        ResetFovPos();
        yield return new WaitForSeconds(downTimeBetweenMovements);
        StartCoroutine(Patrol_cr(waitTime));
    }
    private void OnDrawGizmos()
    {
        graphicSphere1 = transform.position;
        float dist = moveSpeed * moveForTime;
        graphicSphere2 = new Vector2(startFacingRight ? (graphicSphere1.x + dist + (moveSpeed / 10 / moveForTime) - 1) : (graphicSphere1.x - dist - (moveSpeed / 10 / moveForTime) + 1), transform.position.y);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(graphicSphere1, 0.2f);
        Gizmos.DrawWireSphere(graphicSphere2, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(graphicSphere1, graphicSphere2);
    }

    protected override bool NoticeCondition()
    {
        //has no extra condition to get noticed (just has to step in cone)
        return false;
    }

    /*  for(float f = 0; f < timeToMove; f += (Time.deltaTime * moveSpeed))
        {
            //lerp the x value only
            float elapsed = f / timeToMove;
            float x = Mathf.Lerp(start.x, end.x, elapsed);
            Vector3 rbV = _rb.position;
            rbV.x = x;
            rbV.y -= Physics2D.gravity.y;
            _rb.MovePosition(rbV);
            //_rb.MovePosition(new Vector2(x, _rb.position.y));
            //_rb.AddForce(speed * Time.fixedDeltaTime * dir, ForceMode2D.Impulse);
            yield return null;
            if (Vector2.Distance(transform.position, end) <= 0.001f)
            {
                break;
            }
        }*/
}
