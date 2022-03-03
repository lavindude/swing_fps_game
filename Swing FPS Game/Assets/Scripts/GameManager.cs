using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine.Networking;

//use this: https://dev.to/mpetrinidev/call-github-graphql-api-using-c-50o5

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Task t = WebConnection();
        t.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static async Task WebConnection()
    {
        //UnityWebRequest
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://swing-api.herokuapp.com/graphql")
        };

        var queryObject = new
        {
            query = @"query {
                getConnectedPlayers {
                    id
                }
            }"
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonConvert.SerializeObject(
                queryObject), Encoding.UTF8, "application/json")
        };

        dynamic responseObj;
        using (var response = await httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);
        }

        Debug.Log(responseObj.data.getConnectedPlayers.id);
    }
}
