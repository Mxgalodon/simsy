using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeeds : MonoBehaviour
{
    public static PlayerNeeds Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    
}
