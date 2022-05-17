using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
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

    //local data for multiplayer
    private int lobbyId;
    private int playerId;
    public GameObject otherPlayerPrefab;
    private int[] otherPlayerIds;

    // Start is called before the first frame update
    void Start()
    {
        // hard coded values ---------------
        playerId = 1;
        otherPlayerIds = new int[] { 2, 3, 4 };
        lobbyId = 1;
        // hard coded values ---------------
        EnemyObjectData.setEnemyArrayLength(otherPlayerIds.Length);

        for (int i = 0; i < otherPlayerIds.Length; i++)
        {
            GameObject newEnemy = Instantiate(otherPlayerPrefab, new Vector3(2, 48, 0), otherPlayerPrefab.transform.rotation);
            EnemyObjectData.fillUpEnemyArray(newEnemy, otherPlayerIds[i], i);
        }

        InvokeRepeating("SyncOtherPlayers", 0, 0.04f);
    }

    // Update is called once per frame
    void Update()
    {
        //SyncOtherPlayers();
    }

    void SyncOtherPlayers()
    {
        StartCoroutine(OtherPlayerMovement());
    }

    IEnumerator OtherPlayerMovement() // not in APIHelper because cannot use IEnumerator
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