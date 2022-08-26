using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    [Header("GameObjects")]
    [SerializeField] private GameObject _yellingPrefab;
    [SerializeField] private Transform _parent;

    public bool YellingSpawned;
    private Vector3 _offset = new Vector3(-1.5f, 1, 2);

    [SerializeField] private Request[] _requests;

    #region Singleton Pattern
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void ScanForCleaning()
    {
        for (int i = 0; i < WaitingDetection.Instance.waitingCarList.Count; i++)
        {
            CarController car = WaitingDetection.Instance.waitingCarList[i].GetComponent<CarController>();
            
            if (car.doesWantToClean)
            {
                GameObject chat = SpawnYelling(car.transform);
                car.Yelling = chat;
                YellingSpawned = true;
                chat.GetComponent<Rigidbody2D>().freezeRotation = true;
            }
        }
    }

    public GameObject SpawnYelling(Transform car)
    {
        int random = Random.Range(0, _requests.Length); //random request selecting
        var message = Instantiate(_yellingPrefab, car.transform.position + _offset, _yellingPrefab.transform.rotation);
        message.transform.SetParent(car.transform);
        var requestOwner = message.transform.GetChild(0);
        requestOwner.transform.GetChild(0).GetComponent<Text>().text = _requests[random].requestOwner + ": ".ToString();
        requestOwner.transform.GetChild(1).GetComponent<Text>().text = _requests[random].requestText.ToString();
        YellingSpawned = true;
        return message;
    }
}
