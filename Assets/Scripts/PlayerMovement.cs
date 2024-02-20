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
    private float stimBoost = 1f;
    [SerializeField] private float stimDuration;
    [SerializeField] private float stimCooldown;

    [SerializeField] private bool canStim = true;
    [SerializeField] private float adsMultiplier;
    private float adsFactor = 1f;

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
    [SerializeField] private float sprintFovMultiplier;
    [SerializeField] private float dashFov;
    [SerializeField] private float wallRunFovMultiplier;
    [SerializeField] private float stimFovMultiplier;
    private float stimFovBoost = 1f;
    [SerializeField] private float adsFovMultipler;
    private float adsFovFactor = 1f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode doubleJumpkey = KeyCode.Q;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [SerializeField] private KeyCode dashKey = KeyCode.E;
    [SerializeField] private KeyCode stimKey = KeyCode.C;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded = true;
    private string groundTag;
    private RaycastHit groundHitInfo;
    private bool closeToGround;
    private bool canPlayLandEffect;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Dashing")]
    private bool dashing = false;
    private bool canDash = true;

    [Header("ADS")]
    private bool ads = false;
    private Gun Gun_Script;

    [SerializeField] private float aimGlideForce;
    private bool isAimGliding;

    [Header("Audio")]
    [SerializeField] private PlayerAudioManager PAM_Script;



    [SerializeField] private Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private MovementState state;

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

    public void SetGunScript(Gun New_Gun_Script) {
        Gun_Script = New_Gun_Script;
    }

    #region Getters
    public MovementState getMovementState() { return state; }
    public float getHorizontalInput() { return horizontalInput; }
    public float getVerticalInput() { return verticalInput; }
    public float getSprintSpeed() { return sprintSpeed; }
    public float getWalkSpeed() { return walkSpeed; }
    public float getWallRunSpeed() { return wallRunSpeed; }
    public float getMoveSpeed() { return moveSpeed; }
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
    public float getSprintFovMultiplier() { return sprintFovMultiplier; }
    public float getDashFov() { return dashFov; }
    public float getWallRunFovMultiplier() { return wallRunFovMultiplier; }
    public float getStimFovBoost() { return stimFovBoost; }
    public float getADSFovFactor() { return adsFovFactor; }

    public LayerMask getGroundLayer() { return whatIsGround; }
    public bool getGrounded() { return grounded; }
    public bool getDashing() { return dashing; }
    public bool getCanDash() { return canDash; }

    public bool getADS() { return ads; }
    #endregion

    private void MovementStateHandle()
    {
        // wallrunning mode
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallRunSpeed * stimBoost * adsFactor;
        }

        // sprinting mode
        else if (grounded && Input.GetKey(sprintKey) && moveDirection != Vector3.zero)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed * stimBoost * adsFactor;
            cam.DoFov(walkFov * sprintFovMultiplier * stimFovBoost * adsFovFactor);
        }

        // walking mode
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed * stimBoost * adsFactor;
            cam.DoFov(walkFov * stimFovBoost * adsFovFactor);
        }

        // dashing mode
        else if (dashing)
        {
            state = MovementState.dashing;
            moveSpeed = dashSpeed * stimBoost * adsFactor;
        }

        // air mode
        else
        {
            state = MovementState.air;

            if (Input.GetKey(sprintKey))
            {
                moveSpeed = sprintSpeed * stimBoost * adsFactor;
                cam.DoFov(walkFov * sprintFovMultiplier * stimFovBoost * adsFovFactor);
            }

            else
            {
                moveSpeed = walkSpeed * stimBoost * adsFactor;
                cam.DoFov(walkFov * stimFovBoost * adsFovFactor);
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
        GroundedCheck();
        GroundAudioTagManager();

        MyInput();
        SpeedControl();
        MovementStateHandle();
        MovementStim();
        ADSHandler();
        AimGlideHandle();
        HandleDrag();
        DoubleJumpReset();
    }

    private void GroundedCheck()
    {
        if (grounded == false)
        {
            canPlayLandEffect = true;
        }

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        closeToGround = Physics.Raycast(transform.position, Vector3.down, out groundHitInfo, playerHeight * 0.5f + 2f, whatIsGround);

        if (grounded && canPlayLandEffect)
        {
            PAM_Script.PlayWalkSound(0f, 0f);
            canPlayLandEffect = false;
        }
    }

    private void GroundAudioTagManager()
    {
        if ((closeToGround && groundHitInfo.transform.tag == "Tile") || wallrunning)
        {
            groundTag = "Tile";
        }
        else if (closeToGround && groundHitInfo.transform.tag == "Grass")
        {
            groundTag = "Grass";
        }

        PAM_Script.WalkingClipManager(groundTag);
    }

    private void HandleDrag()
    {
        // handle drag
        if (grounded && !dashing)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void DoubleJumpReset()
    {
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

                PAM_Script.PlayJumpSound();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            // when to double jump
            if (Input.GetKey(doubleJumpkey) && !grounded && readyToDoubleJump && !wallrunning)
            {
                readyToDoubleJump = false;
                Jump();

                PAM_Script.PlayDoubleJumpSound();
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
            rb.AddForce(moveSpeed * 20f * GetSlopeMoveDirection(), ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveSpeed * 10f * moveDirection.normalized, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveSpeed * 10f * airMultiplier * moveDirection.normalized, ForceMode.Force);

        if (grounded && !dashing && moveDirection != Vector3.zero && (Mathf.Abs(rb.velocity.x + rb.velocity.z) > Mathf.Epsilon))
        {
            PAM_Script.PlayWalkSound(moveSpeed, stimBoost);
        }
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
            rb.AddForce(moveSpeed * -1f * new Vector3(rb.velocity.x, 0f, rb.velocity.z), ForceMode.Force);
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

    private void ADSHandler()
    {
        if (Input.GetMouseButton(1) && !Gun_Script.getGunData().getReloading() && !dashing)
        {
            adsFactor = adsMultiplier;
            adsFovFactor = adsFovMultipler;
            
            ads = true;
        }
        else
        {
            ads = false;
            adsFactor = 1f;
            adsFovFactor = 1f;
        }
    }

    private void AimGlideHandle()
    {
        if (CanAimGlide() && ads) 
        {
            if (!isAimGliding && rb.velocity.y < 0)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 4f, rb.velocity.z);
            rb.AddForce(transform.up * (aimGlideForce), ForceMode.Force);
            isAimGliding = true;
        }
        else
        {
            isAimGliding = false;
        }
    }

    private bool CanAimGlide()
    {
        if (rb.velocity.y > 0 || closeToGround) return false;
        return true;
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

        cam.DoFov(dashFov * stimFovBoost * adsFovFactor);

        PAM_Script.PlayDashSound();

        // dash cooldown timer
        StartCoroutine(DashTimerReset(dashCooldown));

        // jump before dash to avoid ground drag
        rb.AddForce(transform.up * (jumpForce / 2), ForceMode.Impulse);
        
        yield return new WaitForSeconds(waitTime);

        dashing = false;
        cam.DoFov(walkFov * stimFovBoost * adsFovFactor);
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