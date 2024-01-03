using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : CollidableObject
{
    [SerializeField] private float speed, timeBetweenMovements;
    [SerializeField] private Transform startPos, endPos;
    private Vector2 dir, start, end;
    private bool isAbove, isRight;

    private void Start()
    {
        dir = (endPos.position - startPos.position).normalized;
        start = startPos.position;
        end = endPos.position;
        isAbove = start.y > end.y;
        isRight = start.x > end.x;
        Move();
    }
    private void Move()
    {
        StartCoroutine(Move_cr(dir, end));
    }
    private IEnumerator Move_cr(Vector2 dir, Vector2 dest)
    {
        _rb.velocity = dir * speed;
        while(Vector2.Distance(transform.position, dest) > 0.1f) //new distance less than last AND its > 0.01
        {
            if (isRight && transform.position.x < dest.x) break;
            if(isAbove && transform.position.y < dest.y) break;
            print(Vector2.Distance(transform.position, dest));
            yield return null;
        }
        _rb.velocity = Vector2.zero;
        dir = -dir; 
        _rb.position = dest;
        if(isRight) isRight = false;
        if(isAbove) isAbove = false;
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
