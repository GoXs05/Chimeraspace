using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera fpsCam;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject light;


    [SerializeField] private Recoil Recoil_Script;

    [SerializeField] private GunData gunData;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        light.transform.GetComponent<Light>().enabled = true;
        Invoke("LightOff", 0.1f);
        

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }

        Recoil_Script.RecoilFire();
    }

    void LightOff() 
    {
        light.transform.GetComponent<Light>().enabled = false;
    }


}
