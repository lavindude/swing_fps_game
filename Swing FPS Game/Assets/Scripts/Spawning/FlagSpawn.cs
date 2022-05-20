using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagSpawn : MonoBehaviour
{
    public GameObject[] flags;

    // Start is called before the first frame update
    void Start()
    {
        /*List<Vector3> spawnPositions = new List<Vector3>();
        spawnPositions.Add(spawnSetOne[Random.Range(0, spawnSetOne.Count - 1)].position);
        spawnPositions.Add(spawnSetTwo[Random.Range(0, spawnSetTwo.Count - 1)].position);
        spawnPositions.Add(spawnSetThree[Random.Range(0, spawnSetThree.Count - 1)].position);
        spawnPositions.Add(spawnSetFour[Random.Range(0, spawnSetFour.Count - 1)].position);
        spawnPositions.Add(spawnSetFive[Random.Range(0, spawnSetFive.Count - 1)].position);

        for (int i = 0; i < 5; i++)
        {
            GameObject flag = Instantiate(flags[i]);
            flag.transform.position = spawnPositions[i];
        }*/

        InvokeRepeating("SyncFlags", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SyncFlags()
    {
        StartCoroutine(ManageFlags());
    }

    IEnumerator ManageFlags() // *** under construction **
    {
        for (int i = 0; i < EnemyObjectData.otherPlayerObjects.Length; i++)
        {
            if (EnemyObjectData.otherPlayerObjects[i] != null)
            {
                int userId = EnemyObjectData.otherPlayerObjects[i].enemyId;
                string baseURL = "http://rest-swing-api.herokuapp.com";
                string api_url = baseURL + "/getPosition?userId=" + userId;
                UnityWebRequest request = UnityWebRequest.Get(api_url);

                yield return request.SendWebRequest();

                string json = request.downloadHandler.text;
                PlayerPosition playerPosition = JsonUtility.FromJson<PlayerPosition>(json);
                EnemyObjectData.otherPlayerObjects[i].setOtherPlayerPrefab(new Vector3(playerPosition.positionX,
                                                                            playerPosition.positionY, playerPosition.positionZ));
            }
        }

        yield return null;
    }
}
