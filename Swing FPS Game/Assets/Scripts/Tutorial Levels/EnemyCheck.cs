using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (GameObject.FindObjectOfType<EnemyHealth>() == null)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
