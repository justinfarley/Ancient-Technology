using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//MAYBE NOT LOL: base class
//MAYBE NOT LOL: will be derived for the teleport detecting enemies, jump detecting, etc. Enemies that use a radius to detect instead of a cone will inherit this class
public class RadiusDetectorEnemy : Enemy
{
    [SerializeField] private Detectable detectableType;
    [SerializeField] private Sprite radiusSprite;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float radius;
    public enum Detectable
    {
        Jump, //triggered if player jumps in radius
        Teleport, //triggered if user teleports in radius
        Camo,
        AnyAbility, //triggered if any ability is used within radius
    }
    protected override void Start()
    {
        base.Start();
        SpriteRenderer sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr.sprite = radiusSprite; //use radius sprite
        sr.gameObject.transform.localScale = new Vector3(radius, radius, sr.gameObject.transform.localScale.z);
        isDetector = true;
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Die()
    {
        base.Die();
    }
    protected override bool NoticeCondition()
    {
        return DetectedPlayer();
    }
    private bool DetectedPlayer()
    {
        PlayerMovement player = GetPlayerInRadius();
        if (player == null) return false;
        switch (detectableType)
        {
            case Detectable.Jump:
                return Input.GetKeyDown(player.GetJumpKey());
            case Detectable.Teleport:
                return player.GetComponent<Teleport>().GetCanUseAbility();
            case Detectable.Camo:
                return player.GetComponent<Camo>().GetCanUseAbility();
            case Detectable.AnyAbility:
                foreach(var ability in player.GetComponents<Ability>())
                {
                    if (ability.GetCanUseAbility())
                        return true;
                }
                return false;
            default:
                return false;
        }
    }
    private PlayerMovement GetPlayerInRadius()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerMask);

        if (hits.Length > 0)
            return hits[0].GetComponent<PlayerMovement>();
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public override void CollisionEnter(Collision2D collision)
    {
        base.CollisionEnter(collision);
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
    public override void TriggerEnter(Collider2D collision)
    {
        base.TriggerEnter(collision);
    }
}
