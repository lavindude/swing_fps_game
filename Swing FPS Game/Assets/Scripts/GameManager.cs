using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System;
//using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
//using UnityEngine.Networking;


//*** Check this: https://github.com/xoofx/UnityNuGet
//use this: https://dev.to/mpetrinidev/call-github-graphql-api-using-c-50o5

public class GameManager : MonoBehaviour
{/*
    [DataContract]
    public class JsonConverter
    {
        public JsonConverter() {}
        public static string Serialize<T>(T item)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new DataContractJsonSerializer(typeof(T)).WriteObject(ms, item);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        public static T Deserialize<T>(string body)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(body);
                writer.Flush();
                stream.Position = 0;
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
            }
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        WebConnection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void WebConnection()
    {
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
            Content = new StringContent(JsonConverter.Serialize(
                queryObject), Encoding.UTF8, "application/json")
        };

        dynamic responseObj;
        using (var response = await httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseObj = JsonConverter.Deserialize<dynamic>(responseString);
        }

        Debug.Log(responseObj.data.getConnectedPlayers.id); 
    }
}
