using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum StatsToRegain
    {
        bored,
        hungry,
        tired,
        cleany,
    }

    public StatsToRegain stat;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerScript>(out PlayerScript player))
        {
            player.RegainStatCoroutineStarter(Enum.GetName(typeof(StatsToRegain), stat));
        }
    }
}
