using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAndCustomers : MonoBehaviour
{
    [SerializeField] private CustomerTypes[] _customerTypeSO;
    internal CustomerTypes CustomerSO;
    internal Car _car;

    private Vector3 _offset = new Vector3(-2, 0, 0);

    public bool CanGetCar, WillTakeTheCar = true;


    private void Awake()
    {
        _car = GetComponentInChildren<Car>();
    }

    private void Start()
    {
        #region Random Customer Type Selecting
        int randomIndex = Random.Range(0, _customerTypeSO.Length);
        CustomerSO = _customerTypeSO[randomIndex];
        #endregion
        _car.SetCustomerType(CustomerSO);

        SpawnCustomer(); //when the package which named it "Cars & Customers" appeared, it should spawn the customer group
    }

    private void SpawnCustomer()
    {
        //the scriptable object cannot spawn with a position and rotation
        foreach (GameObject customer in CustomerSO.customers)
        {
            Instantiate(customer, _car.transform.position+ _offset, Quaternion.identity, transform).SetActive(false);
        }
    }


}
