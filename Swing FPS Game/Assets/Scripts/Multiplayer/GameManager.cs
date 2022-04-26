using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public void setOtherPlayerPrefab(float x, float z)
    {
        Vector3 newPosition = new Vector3(x, enemyPrefab.transform.position.y, z);
        enemyPrefab.transform.position = newPosition;
    }
}

public class GameManager : MonoBehaviour
{
    //local data for multiplayer
    private int playerId;
    public GameObject otherPlayerPrefab;
    private int[] otherPlayerIds;
    private EnemyObject[] otherPlayerObjects;

    // Start is called before the first frame update
    void Start()
    {
        // hard coded values ---------------
        playerId = 1;
        otherPlayerIds = new int[] { 2, 3, 4 };
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
        OtherPlayerMovement();
        //printEnemyObjects();
    }

    /*void SyncOtherPlayers()
    {
        StartCoroutine(OtherPlayerMovement());
    }*/

    public void printEnemyObjects()
    {
        for (int i = 0; i < otherPlayerObjects.Length; i++)
        {
            Debug.Log("---------");
            Debug.Log(otherPlayerObjects[i].enemyId);
        }
    }

    public async void OtherPlayerMovement()
    {
        for (int i = 0; i < otherPlayerObjects.Length; i++)
        {
            int userId = otherPlayerObjects[i].enemyId;
            string baseURL = "http://rest-swing-api.herokuapp.com";
            string api_url = baseURL + "/getPosition?userId=" + userId;
            UnityWebRequest request = UnityWebRequest.Get(api_url);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);
                PlayerPosition playerPosition = JsonUtility.FromJson<PlayerPosition>(json);
                otherPlayerObjects[i].setOtherPlayerPrefab(playerPosition.positionX, playerPosition.positionZ);
            }

            else
            {
                Debug.Log("Fail");
                //i--;
            }
        }
    }

    /*IEnumerator OtherPlayerMovement() // not in APIHelper because cannot use IEnumerator
    {
        for (int i = 0; i < otherPlayerObjects.Length; i++)
        {
            int userId = otherPlayerObjects[i].enemyId;
            string baseURL = "http://rest-swing-api.herokuapp.com";
            string api_url = baseURL + "/getPosition?userId=" + userId;
            UnityWebRequest request = UnityWebRequest.Get(api_url);
            
            yield return request.SendWebRequest();
            string json = request.downloadHandler.text;
            Debug.Log(json);
            PlayerPosition playerPosition = JsonUtility.FromJson<PlayerPosition>(json);
            otherPlayerObjects[i].setOtherPlayerPrefab(new Vector3(playerPosition.positionX, playerPosition.positionY, playerPosition.positionZ));
        }

        yield return new WaitForSeconds(1);
    }*/
}
