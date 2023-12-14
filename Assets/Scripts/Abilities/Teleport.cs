using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO: maybe eventually make like powerups that increase the range of the ability or something idk at the end
/// </summary>
public class Teleport : Ability
{
    [Space(10f)]
    [Header("Teleportation Options")]
    [Range(0.1f, 15f)]
    [SerializeField] private float teleportRadius; //used as radius around player
    [SerializeField] private LayerMask teleportBlockers;
    [SerializeField] private Transform radius;
    [SerializeField] private GameObject effect;
    private LineRenderer lineRenderer;

    public override void Awake()
    {
        base.Awake();
        OnUnlock += () =>
        {
            type = Type.Both;
            locked = false;
            GameManager.instance.HasTeleport = true;
        };
    }
    public override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        if (GameManager.instance.HasTeleport)
        {
            Unlock();
        }
    }
    public override void Update()
    {
        base.Update();
        if (IsLocked()) return;
        if(radius.localScale.magnitude != teleportRadius)
        {
            radius.localScale = new Vector3(teleportRadius, teleportRadius, teleportRadius);
        }

        Vector2 currentTeleportPos = GetTeleportPosition();

        if(CanTeleportToWorldPosition(currentTeleportPos) && Input.GetKeyDown(KeyCode.Mouse0) && CanUseAbility())
        {
            TeleportToPos(currentTeleportPos);
        }
    }

    private bool CanTeleportToWorldPosition(Vector3 worldPos)
    {
        Collider2D[] hitsAtPoint = Physics2D.OverlapPointAll(worldPos, teleportBlockers);

        if (hitsAtPoint.Length > 0) return false;

        return true;
    }
    private void TeleportToPos(Vector2 pos)
    {
        //spawn effect at player and the pos
        Instantiate(effect, transform.position, Quaternion.identity);
        Instantiate(effect, pos, Quaternion.identity);
        transform.position = pos;
        ExhaustAbility();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, teleportRadius);
    }
    private Vector2 GetTeleportPosition()
    {
        Vector2 cursorPos = Input.mousePosition;
        Vector2 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorPos);
        Vector2 pos = transform.position;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.SetPosition(0, pos);
        if (Vector2.Distance(transform.position, cursorWorldPos) <= teleportRadius)
        {
            lineRenderer.SetPosition(1, cursorWorldPos);
            //Debug.DrawLine(pos, cursorWorldPos, Color.red, 0.01f);
            return cursorWorldPos;
        }

        Vector2 direction = (cursorWorldPos - (Vector2)transform.position).normalized;

        Vector2 point = pos + direction * teleportRadius;
        lineRenderer.SetPosition(1, point);
        //Debug.DrawLine(pos, point, Color.red, 0.01f);
        return point;
    }

    public override void AbilityKeyHeld()
    {
        //Show radius, make ability useable
        if(!radius.gameObject.activeSelf)
            radius.gameObject.SetActive(true);
        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;
        canUseAbility = true;
    }
    public override void AbilityKeyUp()
    {
        base.AbilityKeyUp();
        if (!CanUseAbility()) return;
        ResetToLastColor(eyeColor);
        activated = false;
        if (radius.gameObject.activeSelf)
            radius.gameObject.SetActive(false);
        if (lineRenderer.enabled)
            lineRenderer.enabled = false;
        canUseAbility = false;
    }
    public override void ExhaustAbility()
    {
        base.ExhaustAbility();
        radius.gameObject.SetActive(false);
        lineRenderer.enabled = false;
    }
    public override bool CanUseAbility()
    {
        if (!base.CanUseAbility()) return false;
        //any other guard clauses to USE ability
        return true;
    }
    public override void AbilityKeyDown()
    {
        base.AbilityKeyDown();
    }
}
