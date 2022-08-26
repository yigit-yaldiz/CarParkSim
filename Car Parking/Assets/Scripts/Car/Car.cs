using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CustomerTypes Customer;

    public void SetCustomerType(CustomerTypes customer)
    {
        Customer = customer;
    }
}
