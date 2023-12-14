using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : CollidableObject
{
    [SerializeField] private float speed;
    private Vector2 moveDir;
    public Attack Parent { get; set; }
    public float Damage { get; set; }
    public override void CollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    public override void TriggerEnter(Collider2D collision)
    {
    }
    private void Start()
    {
        moveDir = new Vector2(speed, 0);
    }
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * moveDir);
    }
    private void OnDestroy()
    {
        Parent.SetCurrentLazer(null);
        Parent.ExhaustAbility();
    }
}   
