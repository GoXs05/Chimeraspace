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
    [Tooltip("In RPM")] [SerializeField] private float fireRate;
    [SerializeField] private float adsFireRateMultiplier;
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

    public float getFireRate() { return fireRate; }
    public void setFireRate(float newFireRate) { fireRate = newFireRate; }

    public float getAdsFireRateMultiplier() { return adsFireRateMultiplier; }
    public void setAdsFireRateMultiplier(float newAdsFireRateMultiplier) { adsFireRateMultiplier = newAdsFireRateMultiplier; }

    public float getReloadTime() { return reloadTime; }
    public void setReloadTime(float newReloadTime) { reloadTime = newReloadTime; }

    public bool getReloading() { return reloading; }
    public void setReloading(bool newReloadVal) { reloading = newReloadVal; }

    public RecoilInfoStruct getRecoilInfo() { return recoilInfo; }
}