using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance { get; private set; }
    
    [Header("GameObjects")]
    [SerializeField] private Transform _costTextParent;
    [SerializeField] private TextMeshProUGUI _earnedMoneyText;

    [Header("Money Variables")]
    public float earnedMoney;

    [Header("Lists")]
    public List<TextMeshProUGUI> costTexts = new List<TextMeshProUGUI>();
    //[field: SerializeField] private Transform[] parkPoints; //const
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        #region Cost text objects added a list from their parent
        foreach (Transform child in _costTextParent)
        {
            costTexts.Add(child.GetComponent<TextMeshProUGUI>());
        }
        #endregion

        //earnedMoney = PlayerPrefs.GetFloat("EarnedMoney", 0);
    }

    void Update()
    {
        //if (Timer.elapsedTime > Timer._maxTimeInterval)
        //{
        //    //�u anl�k materiali de�i�ebilir ! arabay� 2'ye b�lece�iz !
        //}

        _earnedMoneyText.text = "Money: " + Convert.ToInt32(earnedMoney).ToString();
    }
}
