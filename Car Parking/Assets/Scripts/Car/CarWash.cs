using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWash : MonoBehaviour
{
    private ParticleSystem spikeEffect;
    public bool isOccupied;
    public static CarWash Instance { get; private set; }
  
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spikeEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>() != null)
        {   
            CarController carController = other.GetComponent<CarController>();

            if (carController.isDirty)
            {
                //other.transform.position = transform.position;
                //arabanýn hýzý 0'lanabilir
                StartCoroutine(CarCleaning(3f, carController));
            }
        }
    }

    IEnumerator CarCleaning(float time, CarController cc)
    {
        isOccupied = true;
        yield return new WaitForSeconds(time);
        spikeEffect.Play();
        yield return new WaitForSeconds(time/ 4);
        cc.gameObject.GetComponent<Renderer>().material = WashingQueuing.Instance.cleanCarMat;
        cc.transform.GetChild(0).GetComponent<Renderer>().material = WashingQueuing.Instance.cleanCarMat;

        //WashingQueuing.Instance.washingQueue.RemoveAt(0);
        isOccupied = false;
    }
}
