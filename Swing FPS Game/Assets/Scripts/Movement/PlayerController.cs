using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Grapple grapple;

    [Header("Movement")]
    public float moveSpeed;
    public float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float grappleMultiplier = 0.4f;
    [SerializeField] Transform orientation;

    [Header("Sprinting")]
    public float crouchSpeed = 4;
    public float walkSpeed = 6;
    public float sprintSpeed = 12;
    public float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5;

    [Header("Sliding")]
    public float sprintSlideForce = 10;
    public float walkSlideForce = 5;
    public float slideLength = 0.75f;
    public float currentSlideForce = 0f;
    public float currentSlideTime = 0f;


    [Header("KeyBinds")]
    KeyCode jumpKey = KeyCode.Space;
    KeyCode sprintKey = KeyCode.LeftShift;
    KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Drag")]
    public float groundDrag = 6;
    public float airDrag = 2;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    bool isGrounded;
    float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;


    Rigidbody rb;
    [SerializeField] CapsuleCollider cc;

    RaycastHit slopeHit;

    public bool isSprinting = false;
    public bool isCrouching = false;
    public bool isSliding = false;

    public TextMeshProUGUI chestOpenText;
    public TextMeshProUGUI chestCloseText;


    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.1f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        SetSlideSettings();
        Slide();
        Crouch();

        if (isCrouching || isSliding)
        {
            cc.height = 1.5f;
        }
        else
        {
            cc.height = 2f;
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if (grapple.isGrappling)
        {
            rb.drag = 0;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded && !isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            isSprinting = true;
        }
        else if (isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
            isSprinting = false;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            isSprinting = false;
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope() && !isSliding)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope() && !isSliding)
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (grapple.isGrappling && !isSliding)
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * grappleMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded && !isSliding)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (Input.GetKey(crouchKey) && isGrounded && !isSliding)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    void Slide()
    {
        if (isGrounded && isSliding)
        {
            rb.AddForce(orientation.forward * currentSlideForce, ForceMode.Acceleration);
            currentSlideTime -= Time.deltaTime;
        }

        if (currentSlideTime < 0)
        {
            isSliding = false;
        }
    }

    void SetSlideSettings()
    {
        if ((rb.velocity.magnitude > 0f) && Input.GetKeyDown(KeyCode.LeftControl) && !isSliding && isGrounded && !isCrouching)
        {
            if (isSprinting)
            {
                currentSlideForce = sprintSlideForce;
                currentSlideTime = slideLength;
                isSliding = true;
            }
        }

        if (!isGrounded || Input.GetKeyUp(KeyCode.LeftControl))
        {
            isSliding = false;
        }
    }

    public void SetOpenTextTrue()
    {
        chestOpenText.gameObject.SetActive(true);
    }

    public void SetOpenTextFalse()
    {
        chestOpenText.gameObject.SetActive(false);
    }

    public void SetCloseTextTrue()
    {
        chestCloseText.gameObject.SetActive(true);
    }

    public void SetCloseTextFalse()
    {
        chestCloseText.gameObject.SetActive(false);
    }

    public void SetChestTextFalse()
    {
        chestOpenText.gameObject.SetActive(false);
        chestCloseText.gameObject.SetActive(false);
    }
}
             