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

    public static void SendPlayerDeathReceived(int playerId)
    {
        string api_url = baseURL + "/deathConfirmed?playerId=" + playerId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }

    public static void TakeFlag(int playerId, int lobbyId, int flagNum)
    {
        string api_url = baseURL + "/takeFlag?playerId=" + playerId + "&flagNum=" + flagNum + "&lobbyId=" + lobbyId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }

    public static void ResetPlayerData(int playerId, int lobbyId)
    {
        string api_url = baseURL + "/resetPlayerData?playerId=" + playerId + "&lobbyId=" + lobbyId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }

    public static void SendPlayerWon(int playerId, int lobbyId)
    {
        string api_url = baseURL + "/sendPlayerWon?playerId=" + playerId + "&lobbyId=" + lobbyId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }

    public static void ResetFlagsAndPlayerWon(int lobbyId)
    {
        string api_url = baseURL + "/resetFlagsAndPlayerWon?lobbyId=" + lobbyId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        request.SendWebRequest();
    }
}