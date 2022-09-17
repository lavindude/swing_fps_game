using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject otherPlayerPrefab;
    public static Dictionary<string, GameObject> otherPlayers = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this should only run at the beginning of the game
        if (SocketManager.initEnemiesRetrieved)
        {
            //check otherPlayers dictionary in socket manager
            foreach (KeyValuePair<string, OtherPlayerData> enemy in SocketManager.otherPlayers)
            {
                Vector3 newPos = new Vector3(enemy.Value.xPos, enemy.Value.yPos, enemy.Value.zPos);

                if (otherPlayers.ContainsKey(enemy.Key)) // run in case below function happens faster
                {
                    otherPlayers[enemy.Key].transform.position = newPos;
                }

                else
                {
                    GameObject enemyObject = Instantiate(otherPlayerPrefab, newPos, otherPlayerPrefab.transform.rotation);
                    otherPlayers.Add(enemy.Key, enemyObject);
                }
            }

            SocketManager.initEnemiesRetrieved = false;
        }

        while (SocketManager.playersToUpdate.Count > 0)
        {
            OtherPlayerData playerToSync = SocketManager.playersToUpdate.Dequeue();
            string playerId = playerToSync.id;
            Vector3 newPos = new Vector3(playerToSync.xPos, playerToSync.yPos, playerToSync.zPos);
            // update enemy player
            if (otherPlayers.ContainsKey(playerId))
            {
                otherPlayers[playerId].transform.position = newPos;
            }

            else
            {
                GameObject enemy = Instantiate(otherPlayerPrefab, newPos, otherPlayerPrefab.transform.rotation);
                otherPlayers.Add(playerId, enemy);
            }
        }
    }
}