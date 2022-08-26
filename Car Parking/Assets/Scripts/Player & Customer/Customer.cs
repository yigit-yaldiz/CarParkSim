using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Charboa.Utilities;

[RequireComponent(typeof(Rigidbody))]
public class Customer : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    private Rigidbody _rb;
    private CapsuleCollider _customerCollider;
    private MeshRenderer _customerMesh;

    [Header("GameObjects")]
    private Spawner _spawner;
    public Transform Destination;
    [SerializeField] private GameObject _angryYelling;

    [Header("Booleans")]
    public bool isHittable, isGone, gotTheCar;

    CarsAndCustomers carsCustomers;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _spawner = FindObjectOfType<Spawner>();

        _customerCollider = GetComponent<CapsuleCollider>();
        _customerMesh = GetComponent<MeshRenderer>();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Waiting Room"))
        {
            if (isGone)
            {
                return;
            }

            isGone = true;
            gameObject.SetActive(false);
        }
    }

    protected void OnTriggerEnter(Collider other) //child object trigger
    {
        if (other.CompareTag("Car"))
        {
            if (!isHittable)
            {
                return;
            }

            GameObject yelling = 
                Instantiate(_angryYelling, transform.position + new Vector3(-1.5f, 1, 2), _angryYelling.transform.rotation, other.transform.parent);
            yelling.SetActive(false);

            CarsAndCustomers carsCustomers = other.GetComponentInParent<CarsAndCustomers>();
            CarsAndCustomers parent = GetComponentInParent<CarsAndCustomers>();

            if (carsCustomers == parent)
            {
                yelling.SetActive(false);
                GetInTheCar(other);
                MovementController.isInTheCar = false;
                CheckPassengerSituation(other);
                other.GetComponent<CarController>().isCanRideable = false;
                other.transform.parent.SetParent(null);
            }   
            else
            {
                yelling.SetActive(true);
                Delay.Set(3, () => { yelling.SetActive(false); });
            }
        }
    }

    public void GoToThePoint(Transform point)
    {
        Destination = point;
        agent.SetDestination(point.position);
    }

    private void GetInTheCar(Collider car)
    {
        #region Customer Disappering
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        #endregion
        
        transform.position = car.transform.position;

        CarController vehicle = car.GetComponent<CarController>();
        vehicle.GoToThePoint(_spawner.transform);

        MovementController.Instance.PlayerAppearing();
   
        gotTheCar = true;
        isHittable = false;
    }

    private void CheckPassengerSituation(Collider car)
    {
        if (gotTheCar)
        {
            Transform car_customer = car.transform.parent;
            
            for (int i = 1; i < car_customer.childCount; i++)
            {
                car_customer.GetChild(i).gameObject.SetActive(false);
            }

            print("Gotcha the customers!");
        }
    }

    public void FreezeTheCustomer()
    {
        _rb.freezeRotation = true;
        _rb.constraints = RigidbodyConstraints.FreezePosition;
    }
}