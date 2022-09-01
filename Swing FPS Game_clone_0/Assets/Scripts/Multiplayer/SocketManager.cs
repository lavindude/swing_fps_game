using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

public class SocketManager : MonoBehaviour
{
    public static WebSocket socket;
    public static OtherPlayerData[] otherPlayerDatas;

    public GameObject player;

    public GameObject otherPlayerPrefab;

    public PlayerData playerData;
    private Vector3 prevPosition;
    // Start is called before the first frame update
    void Start()
    {
        prevPosition = player.transform.position;

        socket = new WebSocket("ws://localhost:4000");
        //socket = new WebSocket("ws://websocket-starter-code-multiplayer-websocket-app.bsh-serverconnect-b3c-4x1-162e406f043e20da9b0ef0731954a894-0000.us-south.containers.appdomain.cloud/");
        socket.Connect();

        //WebSocket onMessage function
        socket.OnMessage += (sender, e) =>
        {

            //If received data is type text...
            if (e.IsText)
            {
                //Debug.Log("IsText");
                //Debug.Log(e.Data);
                JObject jsonObj = JObject.Parse(e.Data);

                //Get Initial Data server ID data (From intial serverhandshake)
                if (jsonObj["playerId"] != null)
                {
                    //Convert Intial player data Json (from server) to Player data object
                    PlayerData tempPlayerData = JsonUtility.FromJson<PlayerData>(e.Data);
                    playerData = tempPlayerData;
                    Constants.playerId = playerData.playerId;
                    string playerInitData = "{\"dataType\" : \"initializePlayer\", \"data\" : {\"playerId\" : " +
                                        "\"" + playerData.playerId + "\", \"lobbyId\" : \"" + Constants.lobbyId + "\"}}";
                    socket.Send(playerInitData);
                    Debug.Log(Constants.playerId);
                    return;
                }

                if (jsonObj["otherPlayerData"] != null)
                {
                    otherPlayerDatas = jsonObj["otherPlayerData"].ToObject<OtherPlayerData[]>();
                }
            }

        };

        //If server connection closes (not client originated)
        socket.OnClose += (sender, e) =>
        {
            Debug.Log(e.Code);
            Debug.Log(e.Reason);
            Debug.Log("Connection Closed!");
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.transform.position);
        if (socket == null)
        {
            return;
        }

        //If player is correctly configured, begin sending player data to server if player has moved
        if (player != null && playerData.playerId != "" && player.transform.position != prevPosition)
        {
            prevPosition = player.transform.position;

            //Grab player current position and rotation data
            playerData.xPos = player.transform.position.x;
            playerData.yPos = player.transform.position.y;
            playerData.zPos = player.transform.position.z;

            string playerDataJSON = JsonUtility.ToJson(playerData);
            string playerDataSpecified = "{\"dataType\" : \"playerPositionData\", \"data\" : " + playerDataJSON + "}";
            socket.Send(playerDataSpecified);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            string messageJSON = "{\"message\": \"Some Message From Client\"}";
            socket.Send(messageJSON);
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        socket.Close();
    }

    /*void UpdateOtherPlayers(OtherPlayerData[] otherPlayerDatas)
    {
        // go in a loop and instantiate according to white board
        for (int i = 0; i < otherPlayerDatas.Length; i++)
        {
            Debug.Log(otherPlayerDatas[i].playerId);
            Instantiate(otherPlayerPrefab, new Vector3(otherPlayerDatas[i].xPos,
                                                otherPlayerDatas[i].yPos, otherPlayerDatas[i].zPos), otherPlayerPrefab.transform.rotation);

            /*GameObject enemy1 = GameObject.FindGameObjectWithTag(otherPlayerDatas[i].playerId);
            Debug.Log("Yes2");
            if (true)
            {
                Debug.Log("Yessssirrrrr");
                GameObject enemy = Instantiate(otherPlayerPrefab, new Vector3(otherPlayerDatas[i].xPos,
                                                otherPlayerDatas[i].yPos, otherPlayerDatas[i].zPos), otherPlayerPrefab.transform.rotation);
                enemy.tag = otherPlayerDatas[i].playerId;
            }

            else
            {
                Debug.Log("YEssssirrr");
                Vector3 updatedPos = new Vector3(otherPlayerDatas[i].xPos, otherPlayerDatas[i].yPos, otherPlayerDatas[i].zPos);
                GameObject.FindGameObjectWithTag(otherPlayerDatas[i].playerId).transform.position = updatedPos;
            }

        }
    }*/

}