using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float wallRunSpeed;
    public float dashSpeed;
    public float stimMultiplier;
    private float stimBoost = 1f;
    public float stimDuration;
    public float stimCooldown;

    public bool canStim = true;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float dashCooldown;
    public float dashTime;
    bool readyToJump;

    bool readyToDoubleJump;

    [Header("Camera")]
    public PlayerCam cam;
    public float walkFov;

    public float sprintFov;
    public float dashFov;
    public float wallRunFov;
    public float stimFovMultiplier;
    public float stimFovBoost = 1f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode doubleJumpkey = KeyCode.Q;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public KeyCode dashKey = KeyCode.E;
    private KeyCode stimKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Dashing")]
    public bool dashing = false;
    public bool canDash = true;

    public Transform orientation;

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




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }




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




    private void FixedUpdate()
    {
        MovePlayer();
    }




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




    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }




    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }



    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (playerHeight * 0.5f) + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }



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



    private void StimReset()
    {
        stimBoost = 1f;
        stimFovBoost = 1f;
    }
    
    
    
    private void StimTimerReset()
    {
        canStim = true;
    }
}