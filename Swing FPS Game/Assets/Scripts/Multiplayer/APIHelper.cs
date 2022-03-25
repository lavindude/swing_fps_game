using UnityEngine;
using System.Net;
using System.IO;

public static class APIHelper
{
    //public static string baseURL = "http://localhost:4000"; // local testing 
    public static string baseURL = "http://rest-swing-api.herokuapp.com";

    public static void SyncLocation(int playerId, int lobbyId, float x, float y, float z)
    {
        string api_url = baseURL + "/syncPlayerPosition?playerId=" + playerId + "&lobbyId=" + 
                                                lobbyId + "&x=" + x + "&y=" + y + "&z=" + z;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_url);
        request.GetResponse();
    }

    public static LobbyPlayers[] GetLobbyPlayers(int lobbyId)
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
