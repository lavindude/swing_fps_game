using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawn : MonoBehaviour
{
    public List<Transform> spawnSetOne;
    public List<Transform> spawnSetTwo;
    public List<Transform> spawnSetThree;
    public List<Transform> spawnSetFour;
    public List<Transform> spawnSetFive;
    public List<GameObject> flags;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        spawnPositions.Add(spawnSetOne[Random.Range(0, spawnSetOne.Count - 1)].position);
        spawnPositions.Add(spawnSetTwo[Random.Range(0, spawnSetTwo.Count - 1)].position);
        spawnPositions.Add(spawnSetThree[Random.Range(0, spawnSetThree.Count - 1)].position);
        spawnPositions.Add(spawnSetFour[Random.Range(0, spawnSetFour.Count - 1)].position);
        spawnPositions.Add(spawnSetFive[Random.Range(0, spawnSetFive.Count - 1)].position);

        for (int i = 0; i < 5; i++)
        {
            GameObject flag = Instantiate(flags[i]);
            flag.transform.position = spawnPositions[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
