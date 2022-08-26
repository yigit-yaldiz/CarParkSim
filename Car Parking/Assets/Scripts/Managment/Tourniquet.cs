using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tourniquet : MonoBehaviour
{
    public static Tourniquet Instance { get; private set; }

    [SerializeField] private GameObject _tourniquet;
    [SerializeField] private TextMeshProUGUI _carCountText;
    
    private Vector3 corePoint;
    private Vector3 targetPoint = new Vector3(-4.5f, 0.75f, -10.75f);
    
    public int carCount;

    private CarController _carController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        corePoint = _tourniquet.transform.position;
        carCount = 0;
    }

    private void Update()
    {
        _carCountText.text = "Car Count: " + carCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer") || other.CompareTag("Player"))
        {
            _tourniquet.transform.position = targetPoint;
        }
        else if (other.CompareTag("Car"))
        {
            WaitingDetection.Instance.UpdateQueue();

            _tourniquet.transform.position = targetPoint;
            
            _carController = other.GetComponent<CarController>();

            if (_carController == null)
            {
                return;
            }

            if (_carController.isCounted)
            {
                carCount--;
                _carController.isCounted = false;
            }
            else if (!_carController.isCounted)
            {
                carCount++;
                _carController.isCounted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Customer") || other.CompareTag("Player"))
        {
            _tourniquet.transform.position = corePoint;
        }
        else if (other.CompareTag("Car"))
        {
            _tourniquet.transform.position = corePoint;
        }
    }
    IEnumerator LerpTourniquet(float lerpTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            _tourniquet.transform.position = Vector3.Lerp(_tourniquet.transform.position, targetPoint, Time.deltaTime * elapsedTime / lerpTime);
            yield return null;
        }

        _tourniquet.transform.position = targetPoint;
    }

    IEnumerator LerpToNormal(float lerpTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            _tourniquet.transform.position = Vector3.Lerp(targetPoint, _tourniquet.transform.position, Time.deltaTime * elapsedTime / lerpTime);
            yield return null;
        }

        _tourniquet.transform.position = targetPoint;
    }
}
