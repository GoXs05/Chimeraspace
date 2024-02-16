using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RecoilInfoStruct
{
    [SerializeField] private Vector3 recoil;
    [SerializeField] private Vector3 ADSRecoil;
    [SerializeField] private Vector3 posRecoil;
    [SerializeField] private Vector3 ADSPosRecoil;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float posSnappiness;
    [SerializeField] private float posReturnSpeed;

    public Vector3 getRecoil() { return recoil; }
    public Vector3 getADSRecoil() { return ADSRecoil; }
    public Vector3 getPosRecoil() { return posRecoil; }
    public Vector3 getADSPosRecoil() { return ADSPosRecoil; }
    public float getSnappiness() { return snappiness; }
    public float getReturnSpeed() { return returnSpeed; }
    public float getPosSnappiness() { return posSnappiness; }
    public float getPosReturnSpeed() { return posReturnSpeed; }
}
