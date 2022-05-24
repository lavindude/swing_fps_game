using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab)))
        {
            gameObject.SetActive(false);

        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
