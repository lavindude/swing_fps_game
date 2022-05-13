using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Networking;

public static class APIHelper
{
    //public static string baseURL = "http://localhost:4000"; // local testing 
    public static string baseURL = "http://rest-swing-api.herokuapp.com";

    public static void SyncLocation(int playerId, int lobbyId, float x, float y, float z)
    {
        string api_url = baseURL + "/syncPlayerPosition?playerId=" + playerId + "&lobbyId=" +
                                                lobbyId + "&x=" + x + "&y=" + y + "&z=" + z;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }

    public static void DealDamage(int playerId, int damage)
    {
        string api_url = baseURL + "/dealDamage?playerId=" + playerId + "&damage=" + damage;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }

    // ------------------------------------------------
    public static LobbyPlayers[] GetLobbyPlayers(int lobbyId) // this function is under construction **
    {
        string api_url = baseURL + "/getLobbyPlayers?lobbyId=" + lobbyId;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        //split string into an array of JSONs (**there could be bugs here, watch out)
        string[] words = json.Split('}');
        LobbyPlayers[] lobbyArray = new LobbyPlayers[words.Length-1];
        for (int i = 0; i < words.Length-1; i++)
        {
            string item = words[i].Substring(1) + '}';
            LobbyPlayers jsonItem = JsonUtility.FromJson<LobbyPlayers>(item);
            lobbyArray[i] = jsonItem;
        }

        return lobbyArray;
    }
}
