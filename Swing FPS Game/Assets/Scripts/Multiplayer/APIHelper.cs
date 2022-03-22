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
}
