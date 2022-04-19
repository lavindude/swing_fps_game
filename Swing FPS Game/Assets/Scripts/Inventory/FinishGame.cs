using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public int flagsNum;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (player.GetComponent<Inventory>().inventory.Count == flagsNum)
        {
            Debug.Log("Finished Game");
        }
    }
}
