using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    
    public TextMeshProUGUI chestOpenText;
    public TextMeshProUGUI chestCloseText;

    public TextMeshProUGUI playerHealthText;

    public bool wonGame;
    public TextMeshProUGUI finishGameText;

    float playerHeight = 2f;

    public float playerHealth;
    public int oldPlayerHealth;
    public sprite damageSprite;

    [SerializeField] Grapple grapple;
    [SerializeField] private bool useFootsteps = true;

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

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.8f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioSource swingingAudioSource = default;
    [SerializeField] private AudioClip[] asphaltClips = default;
    [SerializeField] private AudioClip[] metalicClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Camera cam;

    Rigidbody rb;
    [SerializeField] CapsuleCollider cc;

    RaycastHit slopeHit;

    public bool isSprinting = false;
    public bool isCrouching = false;
    public bool isSliding = false;

    //local data for multiplayer
    private int playerId;
    private int lobbyId;

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
        cam = GetComponentInChildren<Camera>();

        playerId = Constants.playerId;
        lobbyId = Constants.lobbyId;

        APIHelper.SyncLocation(playerId, lobbyId, transform.position.x, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        UpdateSound();
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
        SyncPlayer();
        PlayerWon();
        PlayerDamage();

        if (isCrouching || isSliding)
        {
            cc.height = 1.5f;
        }
        else
        {
            cc.height = 2f;
        }

        if (useFootsteps)
        {
            HandleFootsteps();
        }

        playerHealthText.text = (playerHealth / 3) + " / " + 100;
    }

    void PlayerWon()
    {
        StartCoroutine(CheckPlayerWon());
    }

    void SyncPlayer()
    {
        StartCoroutine(PlayerMovement());
        StartCoroutine(CheckHealth());
    }

    void PlayerDamage()
    {
        damageSprite.enabled = true;
        
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
    IEnumerator PlayerMovement()
    {
        APIHelper.SyncLocation(playerId, lobbyId, transform.position.x, transform.position.y, transform.position.z);
        yield return null;
    }

    IEnumerator CheckHealth()
    {
        string baseURL = "http://rest-swing-api.herokuapp.com";
        string api_url = baseURL + "/getHealth?playerId=" + playerId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);

        yield return request.SendWebRequest();

        string json = request.downloadHandler.text;
        RespawnData respawnData = JsonUtility.FromJson<RespawnData>(json);
        playerHealth = respawnData.health;
        if (respawnData.health <= 0)
        {
            gameObject.GetComponent<Inventory>().inventory.Clear();
            gameObject.GetComponent<Inventory>().resetFlagImages();
            transform.position = new Vector3(respawnData.startX, respawnData.startY, respawnData.startZ);
            APIHelper.SendPlayerDeathReceived(playerId);
        }

        yield return null;
    }

    IEnumerator CheckPlayerWon()
    {
        string baseURL = "http://rest-swing-api.herokuapp.com";
        string api_url = baseURL + "/getPlayerWon?lobbyId=" + lobbyId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);

        yield return request.SendWebRequest();

        string json = request.downloadHandler.text;
        PlayerWonData playerWonData = JsonUtility.FromJson<PlayerWonData>(json);
        if (playerWonData.playerWon != -1)
        {
            finishGameText.text = "Player " + playerWonData.playerWon + " Wins";
        } else
        {
            finishGameText.text = "";
        }

        yield return null;
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

    void HandleFootsteps()
    {
        if (!isGrounded)
        {
            if (!swingingAudioSource.isPlaying)
            {
                swingingAudioSource.Play();
            }
            else
            {
                return;
            }
        }
        else
        {
            StartCoroutine(FadeOut(swingingAudioSource, 5));

        }
        if (rb.velocity.magnitude <= 0.5)
        {
            return;
        }
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.9f && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.9f)
        {
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if(Physics.Raycast(cam.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Metalic":
                        footstepAudioSource.PlayOneShot(metalicClips[Random.Range(0, metalicClips.Length - 1)]);
                        break;
                    case "Footsteps/Asphalt":
                        footstepAudioSource.PlayOneShot(asphaltClips[Random.Range(0, asphaltClips.Length - 1)]);
                        break;
                    case "Footsteps/Grassy":
                        footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(asphaltClips[Random.Range(0, asphaltClips.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }
    void UpdateSound()
    {
        swingingAudioSource.volume = Mathf.InverseLerp(20.0f, 60.0f, rb.velocity.magnitude);
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
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
             