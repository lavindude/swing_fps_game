using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private float sensX;
    private float sensY;
    [SerializeField] private float normalSensX;
    [SerializeField] private float normalSensY;
    [SerializeField] private float zoomSensX;
    [SerializeField] private float zoomSensY;
    [SerializeField] PlayerController playerController;
    [SerializeField] Wallrun wallrun;

    [Header("FOV")]
    [SerializeField] private float fov;
    [SerializeField] private float sprintFov;
    [SerializeField] private float slideFov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float fovTime;
    [SerializeField] private float zoomFov;

    Camera cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.1f;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        myInput();
        changeFov();

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, wallrun.tilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void myInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    public void changeFov()
    {
        if (Input.GetMouseButton(1))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFov, fovTime * Time.deltaTime);
            sensX = zoomSensX;
            sensY = zoomSensY;
        }
        else if (playerController.isSprinting)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFov, fovTime * Time.deltaTime);
            sensX = normalSensX;
            sensY = normalSensY;
        }
        else if (wallrun.isWallRunning)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, fovTime * Time.deltaTime);
            sensX = normalSensX;
            sensY = normalSensY;
        }
        else if (playerController.isSliding)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, slideFov, fovTime * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovTime * Time.deltaTime);
            sensX = normalSensX;
            sensY = normalSensY;
        }
    }
}
