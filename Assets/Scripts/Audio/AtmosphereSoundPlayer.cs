using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float soundDuration;

    // Update is called once per frame
    void Start()
    {
        PlaySound();
    }

    private void PlaySound()
    {
        source.PlayOneShot(clip);
        Invoke("PlaySound", soundDuration);
    }
}
