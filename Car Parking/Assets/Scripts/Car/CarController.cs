using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    private Rigidbody _rb;
    
    [HideInInspector]
    public GameObject Yelling;

    [HideInInspector]
    public Transform destinationTransform;
    [HideInInspector]
    public bool isDirty, didActivedtheCostumers;

    public bool isCounted, isCanRideable, doesWantToClean;

    private const float distanceTreshold = 0.1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        DirtyDefinition();

        //if (isOnVale)
        //{
        //    Invoke(nameof(ParkonthePoint), 1f);
        
        //    if (doesWantToClean)
        //    {
        //        WashingQueuing.Instance.washingQueue.Add(this);
        //    }
        //}
    }

    void Update()
    {
        //valedeyse yapacak
        //if (doesWantToClean && !CarWash.Instance.isOccupied && WashingQueuing.Instance.washingQueue.Count > 0)
        //{
        //    WashingQueuing.Instance.washingQueue[0].agent.SetDestination(WashingQueuing.Instance.washingTransform.position);
        //}
    }

    public void GoToQueue(Transform transform)
    {
        agent.SetDestination(transform.position);

        StartCoroutine(StopWhenArrived());

        if (!MessageManager.Instance.YellingSpawned)
        {
            MessageManager.Instance.ScanForCleaning();
        }

        IEnumerator StopWhenArrived()
        {
            while (true)
            {
                yield return null;

                float distance = Vector3.Distance(this.transform.position, transform.position);

                float distance_Y = Mathf.Abs(this.transform.position.y - transform.position.y);

                if (distance - distance_Y <= distanceTreshold)
                {
                    ResetNavMesh();
                    FreezeTheCar();
                    isCanRideable = true;
                    break;
                }
            }
        }
    }

    public void GoToThePoint(Transform transform)
    {
        agent.SetDestination(transform.position);
    }
    public void ResetNavMesh()
    {
        agent.ResetPath(); 
    }
    private bool DirtyDefinition()
    {
        int rand = Random.Range(0, 2);

        if (rand == 1)
        {
            isDirty = true;
            CleaningDefition();
        }
        else
        {
            isDirty = false;
            GetComponent<Renderer>().material = WashingQueuing.Instance.cleanCarMat;
            transform.GetChild(0).GetComponentInChildren<Renderer>().material = WashingQueuing.Instance.cleanCarMat;
        }
        
        //Debug.Log("Is this car dirty? " + isDirty);

        return isDirty;
    }
    private bool CleaningDefition()
    {
        int rand = Random.Range(0, 2);

        if (rand == 1)
        {
            doesWantToClean = true;
        }
        else
        {
            doesWantToClean = false;
        }

        //Debug.Log("Does it want to clean? " + doesWantToClean);

        return doesWantToClean;
    }
    public void FreezeTheCar()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void ReleaseTheCar()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }
}
