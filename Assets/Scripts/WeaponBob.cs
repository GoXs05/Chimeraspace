using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [SerializeField] private PlayerMovement mover;
    [SerializeField] private Rigidbody rb;

    private float smooth = 10f;
    private float smoothRot = 12f;

    [Header("Bobbing")]
    [SerializeField] private Vector3 travelLimit;
    [SerializeField] private Vector3 bobLimit;
    private float speedCurve;
    private Vector3 bobPosition;
    private float curveSin {get => Mathf.Sin(speedCurve);}
    private float curveCos {get => Mathf.Cos(speedCurve);}

    [SerializeField] private float bobExaggeration;

    [Header("Bob Rotation")]
    [SerializeField] private Vector3 multiplier;
    private Vector3 bobEulerRotation;

    void Update()
    {
        GetInput();
        
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }


    Vector2 walkInput;

    void GetInput(){
        walkInput.x = mover.getHorizontalInput();
        walkInput.y = mover.getVerticalInput();
        walkInput = walkInput.normalized;
    }

    void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void BobOffset()
    {
        if (!mover.getADS())
        {
            speedCurve += Time.deltaTime * (mover.getGrounded() ? (rb.velocity.magnitude)*bobExaggeration : 1f) + 0.01f;
            bobPosition.x = (curveCos * bobLimit.x * (mover.getGrounded() ? 1:0)) - (walkInput.x * travelLimit.x);
            bobPosition.y = (curveSin * bobLimit.y) - (Input.GetAxis("Vertical") * travelLimit.y);
            bobPosition.z = -1f * (walkInput.y * travelLimit.z);
        }
        else
        {
            speedCurve = 0;
            bobPosition = Vector3.zero;
        }
            
    }

    void BobRotation()
    {
        if (!mover.getADS())
        {
            bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
            bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
            bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
        }
        else
        {
            bobEulerRotation = Vector3.zero;
        }
        
    }

}