using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customers", menuName = "Customers")]
public class CustomerTypes : ScriptableObject
{
    public GameObject[] customers;
}
