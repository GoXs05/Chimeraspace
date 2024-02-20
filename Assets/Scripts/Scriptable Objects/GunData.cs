using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private new string name;

    [Header("Shooting")]
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;
    [SerializeField] private float hipFireSpread;

    [Header("Recoil")]
    [SerializeField] private RecoilInfoStruct recoilInfo;

    [Header("Physics")]
    [SerializeField] private float impactForce;
    
    [Header("Reloading")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int magSize;
    [SerializeField] private int totalAmmo;
    [SerializeField] private int maxTotalAmmo;
    private float currentFireRate;
    [Tooltip("In RPM")] [SerializeField] private float fireRate;
    [SerializeField] private float adsFireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool reloading;


    public string getName() { return name; }
    public void setName(string newName) { name = newName; } 

    public float getDamage() { return damage; }
    public void setDamage(float newDamage) { damage = newDamage; }

    public float getMaxDistance() { return maxDistance; }
    public void setMaxDistance(float newMaxDistance) { maxDistance = newMaxDistance; }

    public float getHipFireSpread() { return hipFireSpread; }
    public void setHipFireSpread(float newHipFireSpread) { hipFireSpread = newHipFireSpread; }

    public float getImpactForce() { return impactForce; }
    public void setImpactForce(float newImpactForce) { impactForce = newImpactForce; }

    public int getCurrentAmmo() { return currentAmmo; }
    public void setCurrentAmmo(int newCurrentAmmo) { currentAmmo = newCurrentAmmo; }
    public void DecrementCurrentAmmo() { currentAmmo--; }

    public int getMagSize() { return magSize; }
    public void setMagSize(int newMagSize) { magSize = newMagSize; }

    public int getTotalAmmo() { return totalAmmo; }
    public void setTotalAmmo(int newTotalAmmo) { totalAmmo = newTotalAmmo; }

    public int getMaxTotalAmmo() { return maxTotalAmmo; }
    public void setMaxTotalAmmo(int newMaxTotalAmmo) { maxTotalAmmo = newMaxTotalAmmo; }

    public float getFireRate() { return fireRate; }
    public void setFireRate(float newFireRate) { fireRate = newFireRate; }

    public float getAdsFireRate() { return adsFireRate; }
    public void setAdsFireRate(float newAdsFireRate) { adsFireRate = newAdsFireRate; }

    public float getReloadTime() { return reloadTime; }
    public void setReloadTime(float newReloadTime) { reloadTime = newReloadTime; }

    public bool getReloading() { return reloading; }
    public void setReloading(bool newReloadVal) { reloading = newReloadVal; }

    public RecoilInfoStruct getRecoilInfo() { return recoilInfo; }

    public float getCurrentFireRate() { return currentFireRate; }
    public void setCurrentFireRate(float newFireRate) { currentFireRate = newFireRate; }
}