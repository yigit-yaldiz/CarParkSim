using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Time Variables")]
    private float _elapsedTime = 0f;
    private const float _multiplier = 1.5f; //çarpan
    private const int _maxTimeInterval = 100;

    [Header("Money Variables")]
    private float _cost = 0f;

    [Header("UI Elements")]
    private TextMeshProUGUI costText;
    public GameObject CarIcon;

    protected void OnTriggerEnter(Collider other)
    {
        //can delay add
        costText = CashManager.Instance.costTexts[transform.GetSiblingIndex()];
        costText.gameObject.SetActive(true);
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            bool packagesChoice = other.GetComponentInParent<CarsAndCustomers>().WillTakeTheCar;

            if (!packagesChoice)
            {
                return;
            }

            do
            {
                _elapsedTime += Time.deltaTime;
                _cost = _elapsedTime * _multiplier; //ücret hesaplama
                costText.text = "Cost: " + _cost.ToString("F2");
                GetVisibleTheCarIcon(other);
            }
            while (_elapsedTime == _maxTimeInterval);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GetInvisiableTheCarCaution(other);
            costText.gameObject.SetActive(false);
            CashManager.Instance.earnedMoney += _cost;
            PlayerPrefs.SetFloat("EarnedMoney", CashManager.Instance.earnedMoney);
            _cost = 0f;
            _elapsedTime = 0f;
        }
    }

    public void GetVisibleTheCarIcon(Collider other)
    {
        //içine giren arabanýn customerýnýn isHittable boolený true ise carIcon active olacak
        Transform package = other.transform.parent;
        CarsAndCustomers unit = package.GetComponent<CarsAndCustomers>();
        int index = transform.GetSiblingIndex();

        if (unit.CanGetCar)
        {
            CustomerManager.Instance.CarIconsList[index].SetActive(true);
            unit.CanGetCar = false;
        }
    }

    public void GetInvisiableTheCarCaution(Collider other)
    {
        Transform package = other.transform.parent;
        CarsAndCustomers car_customer = package.GetComponent<CarsAndCustomers>();
        int index = transform.GetSiblingIndex();
        
        if (!car_customer.CanGetCar)
        {
            CustomerManager.Instance.CarIconsList[index].SetActive(false);
        }
    }
}
