using UnityEngine;
using System.Net;
using System.IO;

public static class APIHelper
{
    //public static string baseURL = "http://localhost:4000"; // local testing
    public static string baseURL = "http://rest-swing-api.herokuapp.com"; // production environment
    public static Moved GetMovedData()
    {
        string api_url = baseURL + "/checkMoved";
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(api_url);
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<Moved>(json);
    }

    public static void SetMoved()
    {
        string api_url = baseURL + "/setMoved";
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(api_url);
        request.GetResponse();
    }

    public static void SetNotMoved()
    {
        string api_url = baseURL + "/setNotMoved";
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(api_url);
        request.GetResponse();
    }
}
