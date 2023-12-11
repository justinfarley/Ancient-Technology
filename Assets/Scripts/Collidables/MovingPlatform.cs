using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : CollidableObject
{
    [SerializeField] private float speed, timeBetweenMovements;
    [SerializeField] private Transform startPos, endPos;
    private Vector2 dir, start, end;

    private void Start()
    {
        dir = (endPos.position - startPos.position).normalized;
        start = startPos.position;
        end = endPos.position;
        Move();
    }
    private void Move()
    {
        StartCoroutine(Move_cr(dir, end));
    }
    private IEnumerator Move_cr(Vector2 dir, Vector2 dest)
    {
        _rb.velocity = dir * speed;
        while (Vector2.Distance(transform.position, dest) > 0.01f)
        {
            yield return null;
        }
        _rb.velocity = Vector2.zero;
        dir = -dir; 
        yield return new WaitForSeconds(timeBetweenMovements);
        StartCoroutine(Move_cr(dir, dest == end ? start : end));
    }

    public override void CollisionEnter(Collision2D collision)
    {
    }

    public override void TriggerEnter(Collider2D collision)
    {
    }
}
