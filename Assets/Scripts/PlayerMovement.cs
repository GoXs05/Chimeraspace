using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float stimMultiplier;
    [SerializeField] private float stimBoost;
    [SerializeField] private float stimDuration;
    [SerializeField] private float stimCooldown;

    [SerializeField] private bool canStim = true;

    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    bool readyToJump;

    bool readyToDoubleJump;

    [Header("Camera")]
    [SerializeField] private PlayerCam cam;
    [SerializeField] private float walkFov;
    [SerializeField] private float sprintFov;
    [SerializeField] private float dashFov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float stimFovMultiplier;
    [SerializeField] private float stimFovBoost = 1f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode doubleJumpkey = KeyCode.Q;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [SerializeField] private KeyCode dashKey = KeyCode.E;
    [SerializeField] private KeyCode stimKey = KeyCode.C;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Dashing")]
    private bool dashing = false;
    private bool canDash = true;

    [SerializeField] private Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        dashing,
        air
    }

    public bool wallrunning;
    #endregion

    #region Getters
    public float getSprintSpeed() { return sprintSpeed; }
    public float getWalkSpeed() { return walkSpeed; }
    public float getWallRunSpeed() { return wallRunSpeed; }
    public float getStimMultiplier() { return stimMultiplier; }
    public float getStimBoost() { return stimBoost; }
    public float getStimDuration() { return stimDuration; }
    public float getStimCooldown() { return stimCooldown; }
    public bool getCanStim() { return canStim; }
    public float getGroundDrag() { return groundDrag; }
    public float getJumpForce() { return jumpForce; }
    public float getJumpCooldown() { return jumpCooldown; }
    public float getAirMultiplier() { return airMultiplier; }
    public float getDashCooldown() { return dashCooldown; }
    public float getDashTime() { return dashTime; }

    public float getWalkFov() { return walkFov; }
    public float getSprintFov() { return sprintFov; }
    public float getDashFov() { return dashFov; }
    public float getWallRunFov() { return wallRunFov; }
    public float getStimFovBoost() { return stimFovBoost; }

    public LayerMask getGroundLayer() { return whatIsGround; }
    public bool getGrounded() { return grounded; }
    public bool getDashing() { return dashing; }
    public bool getCanDash() { return canDash; }
    #endregion

    private void MovementStateHandle()
    {
        // wallrunning mode
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallRunSpeed * stimBoost;
        }

        // sprinting mode
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed * stimBoost;
            cam.DoFov(sprintFov * stimFovBoost);
        }

        // walking mode
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed * stimBoost;
            cam.DoFov(walkFov * stimFovBoost);
        }

        // dashing mode
        else if (dashing)
        {
            state = MovementState.dashing;
            moveSpeed = dashSpeed * stimBoost;
        }

        // air mode
        else
        {
            state = MovementState.air;

            if (Input.GetKey(sprintKey))
            {
                moveSpeed = sprintSpeed * stimBoost;
                cam.DoFov(sprintFov * stimFovBoost);
            }

            else
            {
                moveSpeed = walkSpeed * stimBoost;
                cam.DoFov(walkFov * stimFovBoost);
            }
        }

    }

    // called at the start of the program
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // called every frame of the program
    private void Update()
    {

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        MovementStateHandle();
        MovementStim();

        // handle drag
        if (grounded && !dashing)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // double jump reset
        if (grounded || state == MovementState.wallrunning)
            readyToDoubleJump = true;

    }

    // called 50 times per second for physics calculations
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // handles Input for WASD, jumping, double jumping, and dashing
    private void MyInput()
    {
        if (!dashing)
        {
            // get movement input
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // when to jump
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            // when to double jump
            if (Input.GetKey(doubleJumpkey) && !grounded && readyToDoubleJump && !wallrunning)
            {
                readyToDoubleJump = false;
                Jump();
            }

            // when to dash
            if (canDash && Input.GetKey(dashKey))
            {
                StartCoroutine(DashReset(dashTime));
            }
        }
    }

    // controls force additions to player's rigidbody component based on input
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.useGravity = !OnSlope();

        // on slope
        if (OnSlope() && !exitingSlope && !dashing)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    // controls and standardizes speed 
    private void SpeedControl()
    {
        // limit velocity on slope if needed
        if (OnSlope() && rb.velocity.magnitude > moveSpeed && !exitingSlope && !dashing)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else
        {
            // define flat velocity
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        // simulates friction to stop player from "sliding" to a stop when no movement input is given
        if ((grounded || wallrunning) && moveDirection == Vector3.zero && (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon || Mathf.Abs(rb.velocity.z) > Mathf.Epsilon))
        {
            rb.AddForce(new Vector3(rb.velocity.x, 0f, rb.velocity.z) * moveSpeed * -1f, ForceMode.Force);
        }
        
    }

    // handles movement stim boosters
    private void MovementStim()
    {
        // movement stim handling
        if (Input.GetKeyDown(stimKey) && canStim)
        {
            canStim = false;
            stimBoost = stimMultiplier;
            stimFovBoost = stimFovMultiplier;
            
            Invoke(nameof(StimReset), stimDuration);
            Invoke(nameof(StimTimerReset), stimCooldown);
        }
    }

    // handles jump force additions to player's rigidbody component
    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // resets jump bools
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    // handles slope movement
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (playerHeight * 0.5f) + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    // returns angle of slope as a vector3
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    // reset dash state
    private IEnumerator DashReset(float waitTime)
    {
        dashing = true;
        canDash = false;

        cam.DoFov(dashFov * stimFovBoost);

        // dash cooldown timer
        StartCoroutine(DashTimerReset(dashCooldown));

        // jump before dash to avoid ground drag
        rb.AddForce(transform.up * (jumpForce / 2), ForceMode.Impulse);
        
        yield return new WaitForSeconds(waitTime);

        dashing = false;
        cam.DoFov(walkFov * stimFovBoost);
    }

    // reset dash time (cooldown)
    private IEnumerator DashTimerReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canDash = true;
    }

    // reset stim effects
    private void StimReset()
    {
        stimBoost = 1f;
        stimFovBoost = 1f;
    }

    // reset stim cooldown
    private void StimTimerReset()
    {
        canStim = true;
    }
}