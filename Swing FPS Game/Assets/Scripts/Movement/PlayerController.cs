using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float walkSpeed = 6;
    public float sprintSpeed = 12;
    public float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5;

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

    RaycastHit slopeHit;

    public bool isSprinting = false;
    public bool isCrouching = false;

    //local data for multiplayer
    private Vector3 curPosition;
    private int playerId;
    private int lobbyId;
    public GameObject otherPlayerPrefab;
    private GameObject[] otherPlayers;

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

        curPosition = transform.position;
        playerId = 1;
        lobbyId = 1;

        APIHelper.SyncLocation(playerId, lobbyId, transform.position.x, transform.position.y, transform.position.z);

        LobbyPlayers[] lobbyPlayers = APIHelper.GetLobbyPlayers(lobbyId);
        Debug.Log(lobbyPlayers.Length); // IN PROGRESS
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);

        MyInput();
        curPosition = transform.position;
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        Crouch();
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
        
        if (transform.position != curPosition) //check for movement, if movement send to API
        {
            APIHelper.SyncLocation(playerId, lobbyId, transform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (grapple.isGrappling)
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * grappleMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
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
        if (Input.GetKey(crouchKey) && isGrounded)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }
}
