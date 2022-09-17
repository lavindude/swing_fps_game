using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

public class SocketManager : MonoBehaviour
{
    public static WebSocket socket;

    public static Dictionary<string, OtherPlayerData> otherPlayers = new Dictionary<string, OtherPlayerData>();
    public static bool syncedOtherPlayer = true; // this is for GameManager.cs to reference
    public static string playerToSync;
    public static bool initEnemiesRetrieved = false;

    public static Queue<OtherPlayerData> playersToUpdate = new Queue<OtherPlayerData>();

    public GameObject player;

    public PlayerData playerData;
    private Vector3 prevPosition;
    // Start is called before the first frame update
    void Start()
    {
        prevPosition = player.transform.position;

        socket = new WebSocket("ws://localhost:4000");
        //socket = new WebSocket("ws://swing-backend-v2.herokuapp.com/");
        socket.Connect();

        //WebSocket onMessage function
        socket.OnMessage += (sender, e) =>
        {

            //If received data is type text...
            if (e.IsText)
            {
                JObject jsonObj = JObject.Parse(e.Data);

                //Get Initial Data server ID data (From intial serverhandshake)
                if (jsonObj["playerId"] != null)
                {
                    //Convert Intial player data Json (from server) to Player data object
                    PlayerData tempPlayerData = JsonUtility.FromJson<PlayerData>(e.Data);
                    playerData = tempPlayerData;
                    Constants.playerId = playerData.playerId;
                    socket.Send(SocketCalls.PlayerInitData(playerData.playerId));

                    // get data of all the other players in the lobby prior to joining
                    socket.Send(SocketCalls.GetInitLobbyData());
                    return;
                }

                //if retrieve new data about another player
                if (jsonObj["otherPlayerPosition"] != null)
                {
                    string playerId = jsonObj["otherPlayerPosition"]["id"].ToString();
                    OtherPlayerData otherPlayer = jsonObj["otherPlayerPosition"]["data"].ToObject<OtherPlayerData>();
                    otherPlayer.id = playerId;

                    // add player's new data to queue
                    playersToUpdate.Enqueue(otherPlayer);
                }

                //initialize enemies
                if (jsonObj["enemiesInit"] != null)
                {
                    OtherPlayerData[] enemyPlayers = jsonObj["enemiesInit"].ToObject<OtherPlayerData[]>();
                    foreach (OtherPlayerData enemy in enemyPlayers)
                    {
                        otherPlayers.Add(enemy.id, enemy);
                    }

                    initEnemiesRetrieved = true;
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
            socket.Send(SocketCalls.SendPositionData(playerDataJSON));
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        socket.Close();
    }
}