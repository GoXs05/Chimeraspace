using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarScript : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    public void UpdateUIBar(float maxVal, float currentVal)
    {
        healthBarSprite.fillAmount = currentVal / maxVal;
    }

}
