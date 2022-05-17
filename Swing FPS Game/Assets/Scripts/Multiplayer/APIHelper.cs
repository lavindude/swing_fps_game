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
}