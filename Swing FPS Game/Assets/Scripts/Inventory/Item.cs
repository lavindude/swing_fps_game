using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    public int gunNum;
    public int flagNum;

    public void PickedUp()
    {
        Destroy(gameObject);
    }
}
