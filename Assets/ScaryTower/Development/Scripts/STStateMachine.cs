using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STStateMachine : STMonoBehaviour
{
    [SerializeField] STStateMachineElement[] elements;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}

[Serializable]
public class STStateMachineElement
{
    public STState state;
    public GameObject[] elements;
}