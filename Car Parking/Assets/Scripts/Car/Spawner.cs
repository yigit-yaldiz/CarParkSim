using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{   
    public static Spawner Instance { get; private set; }

    [Header("GameObjects")]
    [SerializeField] private GameObject _carAndCustomer;
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private GameObject _costumerPrefab;
    [SerializeField] private Transform _parkPointsParent;

    [Header("Positions")]
    public Transform _customerDestination;

    [Header("Lists & Variables")]
    public float spawnInterval;
    public static List<Transform> availableParkPoints = new List<Transform>();
    public List<GameObject> SpawnedPackages = new List<GameObject>();
    
    #region Singleton Pattern
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        #region Adding the parkpoints a list from their parent
        foreach (Transform child in _parkPointsParent.transform)
        {
            availableParkPoints.Add(child);
        }
        #endregion

        StartCoroutine(Spawn(spawnInterval));
    }

    IEnumerator Spawn(float spawnInterval)
    {
        for (int i = 0; i < WaitingDetection.Instance.queuePoints.Count; i++)
        {
            SpawnCarAndCustomer();
            #region If the car on a vale
            //int randPosIndex = Random.Range(0, availableParkPoints.Count); //random araba pozisyonu seçme
            //car objesinin içerisinden varýþ noktasýný = availableParkPoints[random].transformuna iletiyoruz
            //if (isOnVale) //vale özelliði açýlýnca gelecek bu nav mash ile þu an manuel ya aynen
            //{
            //    car.GetComponent<CarController>().destinationTransform = availableParkPoints[randPosIndex].transform;
            //}
            //availableParkPoints.RemoveAt(randPosIndex);
            #endregion
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnCarAndCustomer()
    {
        GameObject package = Instantiate(_carAndCustomer, transform.position, Quaternion.identity, transform);
        SpawnedPackages.Add(package);
        CarController car = package.GetComponent<CarsAndCustomers>()._car.GetComponent<CarController>();
        WaitingDetection.Instance.EnqueueCar(car);
    }
}

