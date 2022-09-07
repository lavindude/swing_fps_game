using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    public int gunNum;
    public int flagNum;
    public int ammoAmount;
    public int maxAmmo;

    public void PickedUp()
    {
        Destroy(gameObject);
    }
}
