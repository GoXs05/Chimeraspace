using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Vector3 startPos;

    public float freqFactor;
    public float amplitude;

    void Start()
    {
        startPos = transform.position;
        freqFactor = Random.Range(freqFactor - 0.1f, freqFactor + 0.1f);
    }

    void Update()
    {
        transform.position = startPos + new Vector3(0f, amplitude * Mathf.Sin(Time.time * freqFactor), 0f);
    }
}
