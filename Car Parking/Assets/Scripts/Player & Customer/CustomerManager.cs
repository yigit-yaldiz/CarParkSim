using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }
    
    [SerializeField] private Transform _carIconParent;
    [SerializeField] private Transform _spawner;
    [SerializeField] private Transform _customerWaitingRoom;

    public List<GameObject> CarIconsList = new List<GameObject>();
    internal List<Transform> CarsAndCustomerList = new List<Transform>();
    internal List<Transform> Car_CustomerPackages = new List<Transform>();

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Transform child in _carIconParent)
        {
            CarIconsList.Add(child.gameObject);
        }
    }

    public void ActiveTheCostumers(CarController car)
    {
        if (car.didActivedtheCostumers)
        {
            return; //this guard clause, prevents spawning another customer
        }

        foreach (Transform package in _spawner) //it scans carsAndCustomers packages and add a list
        {
            CarsAndCustomerList.Add(package);
        }

        for (int j = 1; j < CarsAndCustomerList[0].childCount; j++)
        {
            CarsAndCustomerList[0].GetChild(j).gameObject.transform.position = car.transform.position + new Vector3(-2, 0, 0);
            CarsAndCustomerList[0].GetChild(j).gameObject.SetActive(true);
            CarsAndCustomerList[0].GetChild(j).GetComponent<Customer>().GoToThePoint(Spawner.Instance._customerDestination);
        }
       
        car.didActivedtheCostumers = true;
        CarsAndCustomerList.RemoveAt(0); //it provides to scan the child customers in the car-customer package
    }

    public void FindAndAppearTheCustomers()
    {
        foreach (Transform package in _spawner) //it scans carsAndCustomers packages and add a list
        {
            Car_CustomerPackages.Add(package);
        }

        for (int j = 1; j < Car_CustomerPackages[0].childCount; j++)
        {
            #region Appearing and Waiting
            Transform customer = Car_CustomerPackages[0].GetChild(j); //wrong code
            Customer client = customer.GetComponent<Customer>();
            client.gameObject.SetActive(true);
            client.Destination = _customerWaitingRoom;
            client.agent.SetDestination(_customerWaitingRoom.position + new Vector3(3, 0, -2));
            client.FreezeTheCustomer();
            client.isHittable = true;
            #endregion
        }

        Car_CustomerPackages.RemoveAt(0);
    }

    public IEnumerator ClaimOwnCar(CarsAndCustomers package)
    {
        //Customer wants to own car when customer comeOut method runs
        //This appears a icon top of customer's car and you need to bring the car to customer

        int time = Random.Range(5,11);

        Debug.Log("Customer will wait for " + time + " seconds");

        yield return new WaitForSeconds(time); //customer waits in random

        package.CanGetCar = true;

        #region DecideTheCarClaim
        CarsAndCustomers carsAndCustomers = package;
        DecideTheCarClaim(carsAndCustomers);

        if (!carsAndCustomers.WillTakeTheCar)
        {
            yield break;
        }
        #endregion

        FindAndAppearTheCustomers();
    }

    public void DecideTheCarClaim(CarsAndCustomers package)
    {
        int rate = 1;
        int chance = Random.Range(0, 10);

        if (chance < rate || !package.WillTakeTheCar)
        {
            Debug.Log("The customer won't take his/her car.");
            package.WillTakeTheCar = false;   
        }
    }
}
