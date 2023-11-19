using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlockManager : MonoBehaviour
{
    private static List<Ability> unlockedAbilities = new List<Ability>(); //for save data
    private PlayerMovement player;
    private Teleport teleportAbility;
    private Camo camoAbility;
    //TODO: add restof abilities here
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        teleportAbility = player.GetComponent<Teleport>();
        camoAbility = player.GetComponent<Camo>();
        StartCoroutine(UnlockAfterSeconds_cr(teleportAbility, 2));
        StartCoroutine(UnlockAfterSeconds_cr(camoAbility, 4f));
    }
    public static void UnlockAbility(Ability ability)
    {
        ability.Unlock();
        unlockedAbilities.Add(ability);
    }
    //test coroutine
    private IEnumerator UnlockAfterSeconds_cr(Ability ability, float t)
    {
        yield return new WaitForSeconds(t);
        UnlockAbility(ability);
    }
}
