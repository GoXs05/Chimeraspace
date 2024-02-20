using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> tileWalkingClips;
    [SerializeField] private List<AudioClip> grassWalkingClips;
    [SerializeField] private List<AudioClip> hologramWalkingClips;
    [SerializeField] private List<AudioClip> jumpingClips;
    [SerializeField] private List<AudioClip> doubleJumpingClips;
    [SerializeField] private List<AudioClip> dashingClips;

    private List<AudioClip> walkingClips;

    private float timeSinceLastStep = 0f;

    void Update()
    {
        timeSinceLastStep += Time.deltaTime;
    }

    public void WalkingClipManager(string groundTag)
    {
        if (groundTag == "Tile")
            walkingClips = tileWalkingClips;
        else if (groundTag == "Grass")
            walkingClips = grassWalkingClips;
    }

    public void PlayDashSound()
    {
        int randomIndex = Random.Range(0, dashingClips.Count);
        AudioClip clip = dashingClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayJumpSound()
    {
        int randomIndex = Random.Range(0, jumpingClips.Count);
        AudioClip clip = jumpingClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayDoubleJumpSound()
    {
        int randomIndex = Random.Range(0, doubleJumpingClips.Count);
        AudioClip clip = doubleJumpingClips[randomIndex];
        source.PlayOneShot(clip);
    }

    public void PlayWalkSound(float moveSpeed, float stimBoost)
    {
        if (stimBoost == 1)
            {
                if (moveSpeed != 0 && timeSinceLastStep > 2f / moveSpeed)
                {
                    int randomIndex = Random.Range(0, walkingClips.Count);
                    AudioClip clip = walkingClips[randomIndex];
                    source.PlayOneShot(clip);
                    timeSinceLastStep = 0f;
                }
            }
            else
            {
                if (moveSpeed != 0 && timeSinceLastStep > 2f / (moveSpeed * (stimBoost / 1.25)))
                {
                    int randomIndex = Random.Range(0, walkingClips.Count);
                    AudioClip clip = walkingClips[randomIndex];
                    source.PlayOneShot(clip);
                    timeSinceLastStep = 0f;
                }
            }
            if (moveSpeed == 0 && stimBoost == 0)
            {
                int randomIndex = Random.Range(0, walkingClips.Count);
                AudioClip clip = walkingClips[randomIndex];
                source.PlayOneShot(clip);
            }
    }
}
