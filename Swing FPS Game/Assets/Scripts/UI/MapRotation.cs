using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotation : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        transform.rotation.z = player.transform.rotation.y;
    }
}
