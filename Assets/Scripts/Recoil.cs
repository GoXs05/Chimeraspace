using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private Transform camHolder;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Vector3 targetPosition;
    private Vector3 currentPosition;

    private float snappiness;
    private float returnSpeed;
    private float posSnappiness;
    private float posReturnSpeed;
    private Vector3 recoil;
    private Vector3 adsRecoil;
    private Vector3 posRecoil;
    private Vector3 adsPosRecoil;

    private bool ads = false;

    public Vector3 getTargetRotation() { return targetRotation; }
    public Vector3 getCurrentRotation() { return currentRotation; }

    public Vector3 getTargetPosition() { return targetPosition; }
    public Vector3 getCurrentPosition() { return currentPosition; }

    public void setRecoilValues(RecoilInfoStruct recoilInfo) 
    {
        recoil = recoilInfo.recoil;
        adsRecoil = recoilInfo.ADSRecoil;
        posRecoil = recoilInfo.posRecoil;
        snappiness = recoilInfo.snappiness;
        returnSpeed = recoilInfo.returnSpeed;
        posSnappiness = recoilInfo.posSnappiness;
        posReturnSpeed = recoilInfo.posReturnSpeed;
        adsPosRecoil = recoilInfo.ADSPosRecoil;
    }

    public void setADS(bool newAds) { ads = newAds; }

    void Start()
    {
        
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, posReturnSpeed * Time.deltaTime);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, posSnappiness * Time.fixedDeltaTime);
    }

    public void RecoilFire()
    {
        if (!ads)
        {
            targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
            targetPosition += new Vector3(Random.Range(posRecoil.x * 0.5f, posRecoil.x), Random.Range(-posRecoil.y, posRecoil.y), Random.Range(-posRecoil.z, posRecoil.z));
        }
        else
        {
            targetRotation += new Vector3(adsRecoil.x, Random.Range(-adsRecoil.y, adsRecoil.y), Random.Range(-adsRecoil.z, adsRecoil.z));
            targetPosition += new Vector3(Random.Range(adsPosRecoil.x * 0.5f, adsPosRecoil.x), Random.Range(-adsPosRecoil.y, adsPosRecoil.y), Random.Range(-adsPosRecoil.z, adsPosRecoil.z));
        }
        
    }
}
