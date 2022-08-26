using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Request", menuName = "Request")]
public class Request : ScriptableObject
{
    public string requestOwner;
    public string requestText;
}
