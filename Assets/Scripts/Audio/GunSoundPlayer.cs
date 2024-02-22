using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> gunshotClips;
    [SerializeField] private List<AudioClip> reloadClips;
    public void PlayGunShot()
    {
        int randomIndex = Random.Range(0, gunshotClips.Count);
        AudioClip clip = gunshotClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayReloadSound()
    {
        int randomIndex = Random.Range(0, reloadClips.Count);
        AudioClip clip = reloadClips[randomIndex];
        source.PlayOneShot(clip);
    }
}
