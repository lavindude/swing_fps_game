using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float playerHeight = 2f;
    bool isGrounded;

    [Header("Movement")]
    public float moveSpeed = 6;
    public float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Jumping")]
    public float jumpForce = 5;

    [Header("KeyBinds")]
    KeyCode jumpKey = KeyCode.Space;

    [Header("Drag")]
    public float groundDrag = 6;
    public float airDrag = 2;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);

        MyInput();
        ControlDrag();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
