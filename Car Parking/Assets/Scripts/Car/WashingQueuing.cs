using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WashingQueuing : MonoBehaviour
{
    public static WashingQueuing Instance { get; private set; }
    public List<CarController> washingQueue = new List<CarController>();
    public Transform washingTransform;

    [Header("Materials")]
    public Material carDefaultMat;
    public Material cleanCarMat;

    private void Awake()
    {
        Instance = this;
    }
}
