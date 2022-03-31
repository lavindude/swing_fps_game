using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    public int gunNum;

    public void PickedUp()
    {
        Destroy(gameObject);
    }
}
