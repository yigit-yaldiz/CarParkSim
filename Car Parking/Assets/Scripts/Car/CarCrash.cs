using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCrash : MonoBehaviour
{
    [SerializeField] private float _delayTime; // 2

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            CarsAndCustomers parent = other.GetComponentInParent<CarsAndCustomers>();

            if (!parent.WillTakeTheCar)
            {
                StartCoroutine(CrashTheCar(other, _delayTime));                
                //CustomerManager.Instance.Car_CustomerPackages.RemoveAt(0);
            }
            else
            {
                Debug.Log("Customer will take his/her car !!!");
            }
        }
    }

    IEnumerator CrashTheCar(Collider other, float delayTime)
    {
        yield return new WaitForSeconds(delayTime / 2);
        other.transform.SetPositionAndRotation(transform.position, new Quaternion(0, 90, 0, 90));
        MovementController.Instance.PlayerAppearing();
        yield return new WaitForSeconds(delayTime);
        other.transform.localScale = new Vector3(other.transform.localScale.x, 0.4f, other.transform.localScale.z);
        yield return new WaitForSeconds(delayTime / 2);
        other.transform.localScale = new Vector3(other.transform.localScale.x, 0.25f, other.transform.localScale.z);
        yield return new WaitForSeconds(delayTime / 2);
        other.transform.localScale = new Vector3(other.transform.localScale.x, 0.1f, other.transform.localScale.z);
        yield return new WaitForSeconds(delayTime / 2);
        Destroy(other.transform.parent.gameObject);
        CashManager.Instance.earnedMoney += 500;
    }
}
