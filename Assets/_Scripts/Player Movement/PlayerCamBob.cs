using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerCamBob : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _enable = true;
    [SerializeField, Range(0, 0.1f)] private float _Amplitude = 0.003f; [SerializeField, Range(0, 30)] private float _frequency = 25.0f;
    [SerializeField] private Transform _camera = null; [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private float ampFactor = 1f;
    private float freqFactor = 1f;
    private float ADSFactor = 1f;
    [SerializeField] private PlayerMovement mover;
    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        if (!_enable || mover.getMovementState() == PlayerMovement.MovementState.wallrunning || mover.getMovementState() == PlayerMovement.MovementState.dashing) return;
        if (mover.getMovementState() == PlayerMovement.MovementState.sprinting)
        {
            ampFactor = 1f;
            freqFactor = 1f;
        }
        else
        {
            ampFactor = 0.5f;
            freqFactor = 0.5f;
        }

        ADSFactor = mover.getADS() ? 0.1f:1f;
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }
    private Vector3 FootStepMotion()
    {        
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * freqFactor * _frequency) * _Amplitude * ampFactor * ADSFactor;
        pos.x += Mathf.Cos(Time.time * freqFactor * _frequency / 2) * _Amplitude * ampFactor * 2 * ADSFactor;
        return pos;
    }
    private void CheckMotion()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        if (speed < _toggleSpeed) return;
        if (!mover.getGrounded()) return;
 
        PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }
}