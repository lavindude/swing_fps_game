using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyObject
{
    public int enemyId;
    public GameObject enemyPrefab;

    public EnemyObject(int id, GameObject otherPlayerPrefab)
    {
        enemyId = id;
        enemyPrefab = otherPlayerPrefab;
    }

    public void setOtherPlayerPrefab(Vector3 otherPosition)
    {
        enemyPrefab.transform.position = otherPosition;
    }
}

public class GameManager : MonoBehaviour
{
    //local data for multiplayer
    private int lobbyId;
    private int playerId;
    public GameObject otherPlayerPrefab;
    private int[] otherPlayerIds;
    private EnemyObject[] otherPlayerObjects;

    // Start is called before the first frame update
    void Start()
    {
        // hard coded values ---------------
        playerId = 1;
        otherPlayerIds = new int[] { 2, 3 };
        lobbyId = 1;
        // hard coded values ---------------
        otherPlayerObjects = new EnemyObject[otherPlayerIds.Length];

        for (int i = 0; i < otherPlayerIds.Length; i++)
        {
            GameObject newEnemy = Instantiate(otherPlayerPrefab, new Vector3(2, 48, 0), otherPlayerPrefab.transform.rotation);
            EnemyObject newEnemyObject = new EnemyObject(otherPlayerIds[i], newEnemy);
            otherPlayerObjects[i] = newEnemyObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SyncOtherPlayers();
    }

    void SyncOtherPlayers()
    {
        StartCoroutine(OtherPlayerMovement());
    }

    IEnumerator OtherPlayerMovement() // not in APIHelper because cannot use IEnumerator
    {
        for (int i = 0; i < otherPlayerObjects.Length; i++)
        {
            int userId = otherPlayerObjects[i].enemyId;
            string baseURL = "http://rest-swing-api.herokuapp.com";
            string api_url = baseURL + "/getPosition?userId=" + userId;
            UnityWebRequest request = UnityWebRequest.Get(api_url);
            yield return request.SendWebRequest();

            string json = request.downloadHandler.text;
            PlayerPosition playerPosition = JsonUtility.FromJson<PlayerPosition>(json);
            otherPlayerObjects[i].setOtherPlayerPrefab(new Vector3(playerPosition.positionX, playerPosition.positionY, playerPosition.positionZ));
        }

        yield return null;
    }
}
