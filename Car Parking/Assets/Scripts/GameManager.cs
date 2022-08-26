using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _isMultiplied;

    [SerializeField] private TextMeshProUGUI carCountText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TimeButton();
        }
    }

    public void TimeButton()
    {
        if (!_isMultiplied)
        {
            Time.timeScale = 2;
            _isMultiplied = true;
        }
        else
        {
            Time.timeScale = 1;
            _isMultiplied = false;
        }
    }
}
