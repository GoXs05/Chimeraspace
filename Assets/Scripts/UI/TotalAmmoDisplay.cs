using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalAmmoDisplay : MonoBehaviour
{
    private TextMeshProUGUI ui;

    private void Start()
    {
        ui = transform.GetComponent<TextMeshProUGUI>();
    }

    
}
