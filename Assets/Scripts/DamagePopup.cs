using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{

    private void Start()
    {
        DamagePopupHandler.Create(Vector3.zero, 300f);
    }
}
