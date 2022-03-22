using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraRegularPosition;
    [SerializeField] Transform cameraCrouchPosition;
    [SerializeField] PlayerController playerController;
    [SerializeField] float heightChangeSpeed;

    // Update is called once per frame
    void Update()
    {
        if (playerController.isCrouching)
        {
            transform.position = new Vector3(cameraCrouchPosition.position.x, Mathf.Lerp(transform.position.y, cameraCrouchPosition.position.y, heightChangeSpeed * Time.deltaTime), cameraCrouchPosition.position.z);
        }
        else
        {
            transform.position = new Vector3(cameraRegularPosition.position.x, Mathf.Lerp(transform.position.y, cameraRegularPosition.position.y, heightChangeSpeed * Time.deltaTime), cameraRegularPosition.position.z); ;
        }
    }
}
