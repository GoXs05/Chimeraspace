using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    private Transform cooldownTransform;
    private Transform activeIconTransform;
    private PlayerMovement PM_Script;

    private TextMeshProUGUI cooldown;
    
    void Start()
    {
        cooldownTransform = transform.GetChild(0);
        activeIconTransform = transform.GetChild(1);

        cooldown = cooldownTransform.GetComponent<TextMeshProUGUI>();

        activeIconTransform.gameObject.SetActive(true);
        cooldownTransform.gameObject.SetActive(false);
    }

    public void CooldownDisplay(bool isUseable, int cooldownSecs)
    {
        if (!isUseable)
        {
            activeIconTransform.gameObject.SetActive(false);
            cooldownTransform.gameObject.SetActive(true);
            cooldown.text = cooldownSecs.ToString();
        }
        else
        {
            activeIconTransform.gameObject.SetActive(true);
            cooldownTransform.gameObject.SetActive(false);
        }
    }
}
