using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectGrapple();
    }

    void DetectGrapple()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward))
            {
                StartGrapple();
            }
        }
        else
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {

    }

    void StopGrapple()
    {

    }
}
