using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayers
{
    public GameObject otherPlayerPrefab;
    public int otherPlayerId;

    private void setPositions
    {
        this.otherPlayerPrefab.
    }
}

public class GameManager : MonoBehaviour
{
    //local data for multiplayer
    private int lobbyId;
    private int playerId;
    public GameObject otherPlayerPrefab;
    private int[] otherPlayerIds;
    private OtherPlayers[] otherPlayerObjects;

    // Start is called before the first frame update
    void Start()
    {
        // hard coded values ---------------
        playerId = 1;
        otherPlayerIds = new int[] { 2, 3 };
        lobbyId = 1;
        // hard coded values ---------------
        otherPlayerObjects = new OtherPlayers[otherPlayerIds.Length];

        for (int i = 0; i < otherPlayerIds.Length; i++)
        {
            GameObject newEnemy = Instantiate(otherPlayerPrefab, new Vector3(2, 48, 0), otherPlayerPrefab.transform.rotation);
            OtherPlayers newEnemyStruct = new OtherPlayers();
            newEnemyStruct.otherPlayerPrefab = newEnemy;
            newEnemyStruct.otherPlayerId = otherPlayerIds[i];
            otherPlayerObjects[i] = newEnemyStruct;
        }

        StartCoroutine(OtherPlayerMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OtherPlayerMovement()
    {
        for (int i = 0; i < otherPlayerObjects.Length; i++)
        {
            PlayerPosition playerPosition = APIHelper.GetPlayerPosition(otherPlayerObjects[i].otherPlayerId);
            otherPlayerObjects[i].otherPlayerPrefab.transform = new Vector3(playerPosition.positionX, playerPosition.positionY, playerPosition.positionZ);
        }
        yield return null;
    }
}
