using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Automatic Gun", menuName = "Guns/Automatic")]
public class Automatic : Gun
{
    public float fireRate;
    private float lastTimeFired;
    public void OnEnable()
    {
        lastTimeFired = 0f;
    }
    public override void OnLeftMouseHold(Transform cameraPos)
    {
        if (Time.time - lastTimeFired > 1 / fireRate)
        {
            lastTimeFired = Time.time;
            Fire(cameraPos);
            Debug.Log("Hit");        
        }
    }
}
