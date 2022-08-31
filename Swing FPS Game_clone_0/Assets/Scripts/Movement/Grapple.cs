using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] Camera cam;
    public Transform grappleBarrel;
    [SerializeField] LineRenderer lr;
    [SerializeField] private AudioClip[] grappleClips = default;
    public LayerMask borderMask;

    public bool isGrappling;

    Rigidbody rb;

    [Header("Grapple Setting")]
    SpringJoint joint;
    public Vector3 grapplePoint;
    float maxDistance;
    public float spring;
    public float damper;
    public float grapplePullForce;
    public float grapplesLeft;
    [SerializeField] private AudioSource grappleAudio = default;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectGrapple();
    }

    void DetectGrapple()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(2))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, borderMask) && grapplesLeft > 0)
            {
                grappleAudio.PlayOneShot(grappleClips[Random.Range(0, grappleClips.Length - 1)]);
                grapplePoint = hit.point;
                maxDistance = Vector3.Distance(transform.position, hit.point);
                StartGrapple();
            }
        }
        else if (Input.GetMouseButtonUp(2) && isGrappling)
        {
            StopGrapple();
        }
        else if (Input.GetKey(KeyCode.Space) && isGrappling)
        {
            Vector3 direction = (grapplePoint - transform.position).normalized;
            rb.AddForce(direction * grapplePullForce * 0.1f, ForceMode.VelocityChange);

            //maxDistance = maxDistance * 0.1f;
            maxDistance = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = maxDistance;
        }
    }

    void StartGrapple()
    {
        isGrappling = true;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;
        joint.maxDistance = maxDistance;
        joint.minDistance = 0f;
        joint.spring = spring;
        joint.damper = damper;
        grapplesLeft--;
    }

    void StopGrapple()
    {
        isGrappling = false;
        Destroy(joint);
        joint = null;
    }
}
