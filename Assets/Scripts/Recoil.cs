using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;
    [SerializeField] private Transform camHolder;

    public Vector3 getTargetRotation() { return targetRotation; }
    public Vector3 getCurrentRotation() { return currentRotation; }

    public void setRecoilX(float newRecoilX) 
    {
        recoilX = newRecoilX;
    }

    public void setRecoilY(float newRecoilY) 
    {
        recoilY = newRecoilY;
    }

    public void setRecoilZ(float newRecoilZ) 
    {
        recoilZ = newRecoilZ;
    }

    void Start()
    {
        
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
