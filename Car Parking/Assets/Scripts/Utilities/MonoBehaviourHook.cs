using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MonoBehaviourHook : MonoBehaviour   //Hooking Monobehaviour methods
{
    public Action OnStart;
    public Action OnUpdate;

    private void Start()
    {
        OnStart?.Invoke();
    }
    private void Update()
    {
        OnUpdate?.Invoke();
    }
}