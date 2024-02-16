using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    #region Variables
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform camHolder;
    [SerializeField] private Recoil Recoil_Script;

    [SerializeField] private PlayerMovement PM_Script;

    float xRotation;
    float yRotation;
    #endregion

    private void Start()
    {
        // sets cursor to invisible and locks it with screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {


        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * PM_Script.getStimBoost() * PM_Script.getADSFovFactor();
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * PM_Script.getStimBoost() * PM_Script.getADSFovFactor();

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(new Vector3(xRotation, yRotation, 0) + Recoil_Script.getCurrentRotation());
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}