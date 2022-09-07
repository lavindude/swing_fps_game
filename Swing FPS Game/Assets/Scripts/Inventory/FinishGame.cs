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
            APIHelper.SendPlayerWon(Constants.playerId, Constants.lobbyId);
            player.GetComponent<PlayerController>().wonGame = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //reset player won and flags
        if (player.GetComponent<PlayerController>().wonGame)
        {
            APIHelper.ResetFlagsAndPlayerWon(Constants.lobbyId);
            player.GetComponent<Inventory>().inventory.Clear();
            player.GetComponent<Inventory>().resetFlagImages();
        }
        
        player.GetComponent<PlayerController>().wonGame = false;
    }
}
