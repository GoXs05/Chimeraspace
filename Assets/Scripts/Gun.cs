using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private new GameObject light;

    [SerializeField] private Recoil Recoil_Script;
    [SerializeField] private GunData gunData;
    [SerializeField] private PlayerMovement PM_Script;
    private bool ads = false;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private RaycastHit hit;

    private float timeSinceLastShot = 0f;

    private RecoilInfoStruct recoilInfo;

    private GameObject gunHolder;
    private Animator gunHolderAnim;



    public GunData getGunData() { return gunData; }

    public void Start()
    {
        gunData.setReloading(false);
        gunHolder = transform.parent.gameObject;
        gunHolderAnim = gunHolder.GetComponent<Animator>();
    }
    public void StartReload()
    {
        if (!gunData.getReloading() && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private bool CanShoot() => !gunData.getReloading() && timeSinceLastShot > 1f / (gunData.getFireRate() / 60);

    void Shoot()
    {
        muzzleFlash.Play();
        light.transform.GetComponent<Light>().enabled = true;
        Invoke("LightOff", 0.1f);
        
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, gunData.getMaxDistance()))
        {
            // Debug.Log(hitInfo.transform);
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable?.TakeDamage(gunData.getDamage());
            hit = hitInfo;
        }

        Recoil_Script.RecoilFire();
        gunData.DecrementCurrentAmmo();
        timeSinceLastShot = 0;

        // Check for target to apply impact
        TargetHandler();
    }

    private void TargetHandler()
    {
        ITarget target = hit.transform.GetComponent<ITarget>();
        target?.TakeImpact(gunData.getImpactForce(), hit);
    }

    void LightOff() 
    {
        light.transform.GetComponent<Light>().enabled = false;
    }

    private IEnumerator Reload()
    {
        gunData.setReloading(true);

        yield return new WaitForSeconds(gunData.getReloadTime());

        gunData.setCurrentAmmo(gunData.getMagSize());
        gunData.setReloading(false);
    }

    private void GunManager() 
    {
        if (transform.gameObject.activeSelf)
        {
            Recoil_Script.setRecoilValues(gunData.getRecoilInfo());

            PM_Script.SetGunScript(transform.GetComponent<Gun>());

            ads = PM_Script.getADS();
            Recoil_Script.setADS(ads);

            if (ads)
            {
                gunHolderAnim.SetBool("ads", true);
            }
            else
            {
                gunHolderAnim.SetBool("ads", false);
            }
        }
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButton(0) && CanShoot() && gunData.getCurrentAmmo() > 0)
        {
            Shoot();
        }
        if (Input.GetKeyDown(reloadKey) && gunData.getCurrentAmmo() != gunData.getMagSize())
        {
            StartReload();
        }

        GunManager();

        transform.localPosition = Recoil_Script.getCurrentPosition();
    }
}
