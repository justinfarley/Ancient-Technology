using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlockManager : MonoBehaviour
{
    public static Dictionary<int, Ability.Abilities> scriptToAbility = new Dictionary<int, Ability.Abilities>()
    {
        { 2, Ability.Abilities.Teleportation},
    };
    private PlayerMovement player;
    private Teleport teleportAbility;
    private Camo camoAbility;
    private TimeShift timeShiftAbility;
    //private Attack attackAbility;
    //add restof abilities here
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        teleportAbility = player.GetComponent<Teleport>();
        camoAbility = player.GetComponent<Camo>();
        timeShiftAbility = player.GetComponent<TimeShift>();
        if (GameManager.instance.HasTeleport) UnlockAbility(teleportAbility);
        if (GameManager.instance.HasCamo) UnlockAbility(camoAbility);
        if (GameManager.instance.HasTimeSlow) UnlockAbility(timeShiftAbility);
        //StartCoroutine(UnlockAfterSeconds_cr(teleportAbility, 1));
        //StartCoroutine(UnlockAfterSeconds_cr(camoAbility, 1f));
        //StartCoroutine(UnlockAfterSeconds_cr(timeShiftAbility, 1f));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            UnlockAbility(teleportAbility);
        }
        else  if (Input.GetKeyDown(KeyCode.Comma))
        {
            UnlockAbility(camoAbility);
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            UnlockAbility(timeShiftAbility);
        }

    }
    public static void UnlockAbility(Ability ability)
    {
        ability.Unlock();
    }
    //test coroutine
    private IEnumerator UnlockAfterSeconds_cr(Ability ability, float t)
    {
        yield return new WaitForSeconds(t);
        UnlockAbility(ability);
    }
    public Teleport GetTeleport()
    {
        return teleportAbility;
    }
    public Camo GetCamo()
    {
        return camoAbility;
    }
    public TimeShift GetTimeSlow()
    {
        return timeShiftAbility;
    }

}
