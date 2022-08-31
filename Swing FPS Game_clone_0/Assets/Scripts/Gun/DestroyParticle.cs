using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(gameObject);
        }
    }
}
