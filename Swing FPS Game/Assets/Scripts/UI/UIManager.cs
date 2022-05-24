using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject miniMap;
    public GameObject map;
    public bool mapActive;
    // Start is called before the first frame update
    void Start()
    {
        miniMap.SetActive(true);
        map.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            mapActive = true;
        }
        else
        {
            mapActive = false;
        }
        map.SetActive(mapActive);
        miniMap.SetActive(!mapActive);
    }
}
