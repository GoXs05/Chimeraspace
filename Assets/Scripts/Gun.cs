using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject light;

    [SerializeField] private Recoil Recoil_Script;
    [SerializeField] private GunData gunData;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private float timeSinceLastShot = 0f;


    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60);

    void Shoot()
    {
        muzzleFlash.Play();
        light.transform.GetComponent<Light>().enabled = true;
        Invoke("LightOff", 0.1f);
        

        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, gunData.maxDistance))
        {
            Debug.Log(hitInfo.transform.name);
        }

        Recoil_Script.RecoilFire();
        gunData.currentAmmo--;
        timeSinceLastShot = 0;

    }

    void LightOff() 
    {
        light.transform.GetComponent<Light>().enabled = false;
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButton(0) && CanShoot() && gunData.currentAmmo > 0)
        {
            Shoot();
        }
        if (Input.GetKeyDown(reloadKey))
        {
            StartReload();
        }
    }
}
