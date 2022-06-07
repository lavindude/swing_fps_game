using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagSpawn : MonoBehaviour
{
    public GameObject[] flags;
    private int lobbyId;

    // Start is called before the first frame update
    void Start()
    {
        lobbyId = Constants.lobbyId;

        InvokeRepeating("SyncFlags", 0, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SyncFlags()
    {
        StartCoroutine(ManageFlags());
    }

    bool Includes(int[] serverFlags, int item)
    {
        for (int i = 0; i < serverFlags.Length; i++)
        {
            if (serverFlags[i] == item)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator ManageFlags()
    {
        string baseURL = "http://rest-swing-api.herokuapp.com";
        string api_url = baseURL + "/getLobbyFlags?lobbyId=" + lobbyId;
        UnityWebRequest request = UnityWebRequest.Get(api_url);
        yield return request.SendWebRequest();
        string json = request.downloadHandler.text;
        LobbyFlagData flagData = JsonUtility.FromJson<LobbyFlagData>(json);
        int[] serverFlags = flagData.flagsAvailable;

        for (int i = 0; i < flags.Length; i++)
        {
            if (Includes(serverFlags, flags[i].GetComponent<Item>().flagNum))
            {
                flags[i].SetActive(true);
            } else
            {
                flags[i].SetActive(false);
            }
        }

        yield return null;
    }
}
