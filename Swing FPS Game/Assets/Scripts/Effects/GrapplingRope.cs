using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public Grapple grapple;
    public int quality;
    private Spring spring;
    public float damper;
    public float strength;
    public float velocity;
    public float waveHeight;
    public float waveCount;
    public AnimationCurve affectCurve;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        grapple = GetComponent<Grapple>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    void LateUpdate()
    {
        updateLr();
    }

    void updateLr()
    {
        if (!grapple.isGrappling)
        {
            currentGrapplePosition = grapple.grappleBarrel.position;
            spring.Reset();
            if (lr.positionCount > 0)
            {
                lr.positionCount = 0;
            }

            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePoint = grapple.grapplePoint;
        var gunTipPosition = grapple.grappleBarrel.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);
        
        for (int i = 0; i < quality + 1; i++)
        {
            var delta = i / (float) quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value * affectCurve.Evaluate(delta));
            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
