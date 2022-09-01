using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    //local data for multiplayer
    public GameObject otherPlayerPrefab;
    private Dictionary<string, GameObject> otherPlayers = new Dictionary<string, GameObject>();
    private int[] otherPlayerIds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

// Update is called once per frame
    void Update()
    {
        string sendData = "{\"dataType\" : \"getOtherPlayerData\", \"data\" : {\"playerId\" : \"" + Constants.playerId + "\", \"lobbyId\" : " + Constants.lobbyId + "}}";
        SocketManager.socket.Send(sendData);

        SyncOtherPlayers(SocketManager.otherPlayerDatas);
    }

    void SyncOtherPlayers(OtherPlayerData[] otherPlayerDatas)
    {
        for (int i = 0; i < otherPlayerDatas.Length; i++)
        {
            if (otherPlayers.ContainsKey(otherPlayerDatas[i].playerId))
            {
                Vector3 updatedPos = new Vector3(otherPlayerDatas[i].xPos, otherPlayerDatas[i].yPos, otherPlayerDatas[i].zPos);
                otherPlayers[otherPlayerDatas[i].playerId].transform.position = updatedPos;
            }

            else
            {
                GameObject enemy = Instantiate(otherPlayerPrefab, new Vector3(otherPlayerDatas[i].xPos,
                                                otherPlayerDatas[i].yPos, otherPlayerDatas[i].zPos), otherPlayerPrefab.transform.rotation);
                otherPlayers.Add(otherPlayerDatas[i].playerId, enemy);
            }
        }
    }
}