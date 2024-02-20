using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentAmmoUI;
    [SerializeField] private TextMeshProUGUI totalAmmoUI;
    [SerializeField] private TextMeshProUGUI gunNameUI;

    public void SetCurrentAmmoUI(float newAmmo)
    {
        string newAmmoString = "";
        if (newAmmo < 10)
        {
            newAmmoString = "0";
        }
        currentAmmoUI.text = newAmmoString + newAmmo.ToString();
    }

    public void SetTotalAmmoUI(float newAmmo)
    {
        string newAmmoString = "";
        if (newAmmo < 10)
        {
            newAmmoString = "00";
        }
        else if (newAmmo < 100)
        {
            newAmmoString = "0";
        }
        totalAmmoUI.text = newAmmoString + newAmmo.ToString();
    }

    public void SetGunNameUI(string newName)
    {
        gunNameUI.text = newName;
    }
}
