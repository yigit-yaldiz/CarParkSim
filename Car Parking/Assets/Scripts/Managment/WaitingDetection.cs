using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaitingDetection : MonoBehaviour
{
    public static WaitingDetection Instance { get; private set; }

    public Transform _waitingPointsParent;
    
    [Header("Lists")]
    public List<Transform> queuePoints = new List<Transform>();
    public List<CarController> waitingCarList = new List<CarController>();

    private void Awake()
    {
        Instance = this;

        AttachThePoints();
    }

    private void AttachThePoints()
    {
        foreach (Transform point in _waitingPointsParent.transform)
        {
            queuePoints.Add(point);
        }
    }

    public void UpdateQueue()
    {
        for (int i = 0; i < waitingCarList.Count; i++)
        {
            waitingCarList[i].GoToQueue(queuePoints[i].transform);
        }
    }

    public void EnqueueCar(CarController car)
    {
        waitingCarList.Add(car);
        UpdateQueue();
    }

    public void DequeueCar(CarController car)
    {
        CustomerManager.Instance.ActiveTheCostumers(car);
        waitingCarList.Remove(car);
        UpdateQueue();
    }
}
