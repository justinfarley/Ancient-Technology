using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Ability
{
    [SerializeField] private GameObject lazerPrefab;
    [SerializeField] private Transform lazerSpawnRight, lazerSpawnLeft;
    private float damage = 5f;
    private Lazer currentLazer = null;
    private SpriteRenderer playerSR;

    public override void Awake()
    {
        base.Awake();
        OnUnlock += () =>
        {
            type = Type.Both;
            locked = false;
            GameManager.instance.HasAttack = true;
        };

    }
    public override void Start()
    {
        base.Start();
        if (GameManager.instance.HasAttack)
        {
            Unlock();
        }
        playerSR = FindObjectOfType<PlayerMovement>().GetComponent<SpriteRenderer>();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void AbilityKeyHeld()
    {
        
    }
    public override void AbilityKeyUp()
    {
        base.AbilityKeyUp();
    }
    public override void AbilityKeyDown()
    {
        if (CanShoot())
        {
            base.AbilityKeyDown();
            SpawnLazer();
        }
        else
        {
            activated = false;
        }
    }
    public override bool CanUseAbility()
    {
        return base.CanUseAbility();
    }
    public override void ExhaustAbility()
    {
        base.ExhaustAbility();
    }
    private bool CanShoot()
    {
        //TODO: implement conditions
        foreach(var key in GetComponent<Teleport>().GetAbilityKeys())
        {
            if (Input.GetKey(key)) return false;
        }
        if (!GameManager.instance.HasAttack) return false;
        return true;
    }
    private void SpawnLazer()
    {
        if (currentLazer != null) return;
        Vector2 spawnPos;
        //.3 increment
        if (playerSR.flipX) 
        {
            spawnPos = lazerSpawnLeft.position;
            if(playerSR.GetComponent<PlayerMovement>().IsMoving())
                spawnPos.x -= 0.5f;
        }
        else
        {
            spawnPos = lazerSpawnRight.position;
            if (playerSR.GetComponent<PlayerMovement>().IsMoving())
                spawnPos.x += 0.5f;
        }
        currentLazer = Instantiate(lazerPrefab, spawnPos, Quaternion.identity).GetComponent<Lazer>();
        currentLazer.Parent = this;
        currentLazer.Damage = damage;
        if (playerSR.flipX)
            currentLazer.ShootingLeft = true;
    }
    public void SetCurrentLazer(Lazer lazer)
    {
        currentLazer = lazer;
    }
}
