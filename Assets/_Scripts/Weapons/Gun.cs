using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private new GameObject light;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private GameObject bulletHoleContainer;

    [SerializeField] private Recoil Recoil_Script;
    [SerializeField] private GunData gunData;
    [SerializeField] private PlayerMovement PM_Script;
    [SerializeField] private AmmoDisplay AD_Script;
    private bool ads = false;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    [SerializeField] private GameObject crosshair;

    private RaycastHit hit;

    private float timeSinceLastShot = 0f;

    private RecoilInfoStruct recoilInfo;

    private GameObject gunHolder;
    private Animator gunHolderAnim;

    private GunSoundPlayer GSP_Script;



    public GunData getGunData() { return gunData; }

    public void Start()
    {
        gunData.setReloading(false);
        gunHolder = transform.parent.gameObject;

        gunHolderAnim = gunHolder.GetComponent<Animator>();
        GSP_Script = transform.GetComponent<GunSoundPlayer>();

        gunData.setTotalAmmo(gunData.getMaxTotalAmmo());
        gunData.setCurrentAmmo(gunData.getMagSize());

        AD_Script.SetCurrentAmmoUI(gunData.getCurrentAmmo());
        AD_Script.SetTotalAmmoUI(gunData.getTotalAmmo());
        AD_Script.SetGunNameUI(gunData.getName());
    }
    public void StartReload()
    {
        if (!gunData.getReloading() && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private bool CanShoot() => !gunData.getReloading() && timeSinceLastShot > 1f / (gunData.getCurrentFireRate() / 60);

    void Shoot()
    {
        muzzleFlash.Play();
        light.transform.GetComponent<Light>().enabled = true;
        Invoke("LightOff", 0.1f);

        GSP_Script.PlayGunShot();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, gunData.getMaxDistance()))
        {
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable?.TakeDamage(gunData.getDamage());
            hit = hitInfo;

            float posFactor = 0.01f;
            Vector3 spawnPos = new Vector3 (hit.point.x - (ray.direction.x * posFactor), hit.point.y - (ray.direction.y * posFactor), hit.point.z - (ray.direction.z * posFactor));
            GameObject bulletHole = Instantiate(bulletHolePrefab, spawnPos, Quaternion.identity);

            bulletHole.transform.rotation = Quaternion.LookRotation(-hit.normal);
            bulletHole.transform.SetParent(bulletHoleContainer.transform);
        }

        Recoil_Script.RecoilFire();
        gunData.DecrementCurrentAmmo();
        timeSinceLastShot = 0;

        // Check for target to apply impact
        TargetHandler();

        AD_Script.SetCurrentAmmoUI(gunData.getCurrentAmmo());
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

        crosshair.SetActive(false);
        GSP_Script.PlayReloadSound();

        yield return new WaitForSeconds(gunData.getReloadTime());

        if (gunData.getMagSize() <= gunData.getTotalAmmo())
        {
            gunData.setTotalAmmo(gunData.getTotalAmmo() - (gunData.getMagSize() - gunData.getCurrentAmmo()));
            gunData.setCurrentAmmo(gunData.getMagSize());
        }
        else
        {
            gunData.setCurrentAmmo(gunData.getTotalAmmo());
            gunData.setTotalAmmo(0);
        }
        gunData.setReloading(false);

        crosshair.SetActive(true);

        AD_Script.SetCurrentAmmoUI(gunData.getCurrentAmmo());
        AD_Script.SetTotalAmmoUI(gunData.getTotalAmmo());
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
                gunData.setCurrentFireRate(gunData.getAdsFireRate());
            }
            else
            {
                gunHolderAnim.SetBool("ads", false);
                gunData.setCurrentFireRate(gunData.getFireRate());
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
        if ((Input.GetKeyDown(reloadKey) && gunData.getCurrentAmmo() != gunData.getMagSize()) || gunData.getCurrentAmmo() == 0)
        {
            StartReload();
        }

        GunManager();

        transform.localPosition = Recoil_Script.getCurrentPosition();

        if (gunData.getTotalAmmo() == 0)
        {
            gunData.setTotalAmmo(gunData.getMaxTotalAmmo());
        }
    }
}
