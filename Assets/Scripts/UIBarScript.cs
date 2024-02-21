using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarScript : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    private float _maxVal = 10f;
    private float _curVal = 10f;
    public void UpdateUIBar(float maxVal, float currentVal)
    {
        healthBarSprite.fillAmount = currentVal / maxVal;
    }

    private void Start()
    {
        UpdateUIBar(_maxVal, _curVal);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _curVal--;
            UpdateUIBar(_maxVal, _curVal);
        }
    }
}
