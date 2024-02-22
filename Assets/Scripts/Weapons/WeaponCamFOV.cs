using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponCamFOV : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    void Update()
    {
        GetComponent<Camera>().fieldOfView = playerCam.GetComponent<Camera>().fieldOfView;
    }
}
