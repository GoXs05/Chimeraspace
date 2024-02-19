using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] walkingClips;
    [SerializeField] private AudioClip[] jumpingClips;
    [SerializeField] private AudioClip[] doubleJumpingClips;
    [SerializeField] private AudioClip[] dashingClips;

    private float timeSinceLastStep = 0f;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastStep += Time.deltaTime;
    }

    public void PlayDashSound()
    {
        int randomIndex = Random.Range(0, dashingClips.Length);
        AudioClip clip = dashingClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayJumpSound()
    {
        int randomIndex = Random.Range(0, jumpingClips.Length);
        AudioClip clip = jumpingClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayDoubleJumpSound()
    {
        int randomIndex = Random.Range(0, doubleJumpingClips.Length);
        AudioClip clip = doubleJumpingClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayWalkSound(float moveSpeed, float stimBoost)
    {
        if (stimBoost == 1)
        {
            if (moveSpeed != 0 && timeSinceLastStep > 2f / moveSpeed)
            {
                int randomIndex = Random.Range(0, walkingClips.Length);
                AudioClip clip = walkingClips[randomIndex];
                source.PlayOneShot(clip);
                timeSinceLastStep = 0f;
            }
        }
        else
        {
            if (moveSpeed != 0 && timeSinceLastStep > 2f / (moveSpeed * (stimBoost / 1.25)))
            {
                int randomIndex = Random.Range(0, walkingClips.Length);
                AudioClip clip = walkingClips[randomIndex];
                source.PlayOneShot(clip);
                timeSinceLastStep = 0f;
            }
        }
        if (moveSpeed == 0 && stimBoost == 0)
        {
            int randomIndex = Random.Range(0, walkingClips.Length);
            AudioClip clip = walkingClips[randomIndex];
            source.PlayOneShot(clip);
        }
    }
}
