using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System;
//using UnityEngine.Networking;

//use this: https://dev.to/mpetrinidev/call-github-graphql-api-using-c-50o5

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //UnityWebRequest
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://swing-api.herokuapp.com/graphql")
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
