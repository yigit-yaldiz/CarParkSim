using System.Collections;
using System.Reflection;
using UnityEngine;

public class ParkManagment : MonoBehaviour
{
    private MovementController _player;
    
    [Header("Components")]
    private CapsuleCollider _playerCollider;
    private MeshRenderer _playerRenderer;

    private void Awake()
    {
        _player = FindObjectOfType<MovementController>();
        _playerCollider = _player.GetComponent<CapsuleCollider>();
        _playerRenderer = _player.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car"))
        {
            return;
        }

        CarsAndCustomers carsAndCustomers = other.transform.parent.GetComponent<CarsAndCustomers>();
        CustomerManager.Instance.DecideTheCarClaim(carsAndCustomers);

        if (!carsAndCustomers.WillTakeTheCar)
        {
            return;
        }

        StartCoroutine(PlayerExitsFromCar(other, 1f));
    }

    private IEnumerator PlayerExitsFromCar(Collider other, float delayTime)
    {
        WaitingDetection.Instance.UpdateQueue();

        yield return new WaitForSeconds(delayTime);

        CarController carController = other.GetComponentInChildren<CarController>();
        CarsAndCustomers car_customer = other.transform.GetComponentInParent<CarsAndCustomers>();

        #region Player-Car Positioning
        if (transform.position.x < 0)
        {
            carController.transform.SetPositionAndRotation(transform.position, new Quaternion(0, -90, 0, 90));
            _player.transform.position = transform.position + new Vector3(3.5f, 0, 0);
            //customer.canWantHisCar = true;
        }
        else
        {
            carController.transform.SetPositionAndRotation(transform.position, new Quaternion(0, 90, 0, 90));
            _player.transform.position = transform.position + new Vector3(-3.5f, 0, 0);
            //customer.canWantHisCar = true;
        }
        #endregion

        MovementController.isInTheCar = false;
        carController.isCanRideable = true;

        #region Player Settings
        _playerCollider.enabled = true;
        _playerRenderer.enabled = true;
        _player.FreezeThePlayer();
        _player.ReleaseThePlayer();
        #endregion

        ClearConsole(); //clean the console

        StartCoroutine(CustomerManager.Instance.ClaimOwnCar(car_customer));
    }
    
    public void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

}
