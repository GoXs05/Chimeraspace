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

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float dashCooldown;
    public float dashTime;
    bool readyToJump;

    bool readyToDoubleJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode doubleJumpkey = KeyCode.Q;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public KeyCode dashKey = KeyCode.E;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
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
            moveSpeed = wallRunSpeed;
        }

        // sprinting mode
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // walking mode
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // dashing mode
        else if (dashing)
        {
            state = MovementState.dashing;
            moveSpeed = dashSpeed;
        }

        // air mode
        else
        {
            state = MovementState.air;
            moveSpeed = sprintSpeed;
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

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }




    private void SpeedControl()
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




    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }




    private void ResetJump()
    {
        readyToJump = true;
    }




    // reset dash state
    private IEnumerator DashReset(float waitTime)
    {
        dashing = true;
        canDash = false;

        // dash cooldown timer
        StartCoroutine(DashTimerReset(dashCooldown));

        // jump before dash to avoid ground drag
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        yield return new WaitForSeconds(waitTime);

        dashing = false;
    }




    // reset dash time (cooldown)
    private IEnumerator DashTimerReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canDash = true;
    }
}