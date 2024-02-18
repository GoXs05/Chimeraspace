using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    public void PlayGunShot()
    {
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clip = clips[randomIndex];
        source.PlayOneShot(clip);
    }
}
