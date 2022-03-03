using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform grappleBarrel;
    [SerializeField] LineRenderer lr;

    public bool isGrappling;

    [Header("Grapple Setting")]
    SpringJoint joint;
    Vector3 grapplePoint;
    float maxDistance;
    public float spring;
    public float damper;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectGrapple();
    }

    void LateUpdate()
    {
        updateLr();
    }

    void DetectGrapple()
    {
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                grapplePoint = hit.point;
                maxDistance = Vector3.Distance(transform.position, hit.point);
                StartGrapple();
            }
        }
        else if (Input.GetButtonUp("Fire1") && isGrappling)
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        isGrappling = true;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;
        joint.maxDistance = maxDistance;
        joint.spring = spring;
        joint.damper = damper;
    }

    void StopGrapple()
    {
        isGrappling = false;
        Destroy(joint);
        joint = null;
    }

    void updateLr()
    {
        if (isGrappling)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, grapplePoint);
            lr.SetPosition(1, grappleBarrel.position);
        }
        else
        {
            lr.positionCount = 0;
        }
    }
}
