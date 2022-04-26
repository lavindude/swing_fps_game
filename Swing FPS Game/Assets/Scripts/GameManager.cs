using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] chestItems;
    private GameObject player;
    private GameObject playerCapsule;
    private GameObject[] chests;
    private GameObject chest;
    private Animator chestAnim;
    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        chests = GameObject.FindGameObjectsWithTag("Chest");
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");
        playerCapsule = player.transform.Find("Capsule").gameObject;
        pc = player.GetComponent<PlayerController>();

        ChestManager();
    }

    GameObject[] RandomizeItems()
    {
        GameObject[] items = chestItems;
        return items;
    }

    Rigidbody[] GetRigidBodies(GameObject[] items)
    {
        Rigidbody[] itemBodies = new Rigidbody[items.Length];
        for(int i = 0; i < items.Length; i++)
        {
            itemBodies[i] = items[i].GetComponent<Rigidbody>();
        }

        return itemBodies;
    }

    bool GetChestState(Animator chestAnim)
    {
        if (chestAnim.GetInteger("ChestStage") == 0)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    void ChestOpen(Animator chestAnim)
    {
        chestAnim.SetInteger("ChestStage", 1);
    }

    GameObject[] SpawnItems(GameObject spawnChest, GameObject[] chestItems)
    {
        GameObject[] instantiatedItems = new GameObject[chestItems.Length];

        for(int i = 0; i < chestItems.Length; i++)
        {
            instantiatedItems[i] = Instantiate(chestItems[i], spawnChest.transform.position, 
                Quaternion.identity, spawnChest.transform.parent);
        }
        
        return instantiatedItems;
    }

    void LaunchItems(GameObject[] instantiatedItems)
    {
        Rigidbody[] itemBodies = GetRigidBodies(instantiatedItems);

        for(int i = 0; i < itemBodies.Length; i++)
        {
            instantiatedItems[i].SetActive(true);
            itemBodies[i].AddForce(transform.up * 100.0f);
        }
    }

    GameObject FindClosestChest()
    {
        if(chests.Length > 1)
        {
            int index = new int();
            float prevDist = Vector3.Distance(playerCapsule.transform.position, chests[0].transform.position);
            float currDist;
            for(int i = 1; i < chests.Length; i++)
            {
                currDist = Vector3.Distance(playerCapsule.transform.position, chests[i].transform.position);
                if (prevDist > currDist)
                {
                    index = i;
                    prevDist = currDist;
                    
                }
            }

            return chests[index];
        }
        else
        {
            return chests[0];
        }
    }

    void PlayerChest(Animator chestAnim)
    {
        if (Vector3.Distance(playerCapsule.transform.position, chest.transform.position) < 3)
        {
            if (GetChestState(chestAnim))
            {
                pc.SetOpenTextTrue();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ItemLauncher();
                    pc.SetOpenTextFalse();
                    ChestOpen(chestAnim);
                }
            }
        }
        else
        {
            pc.SetChestTextFalse();
        }
    }

    void ItemLauncher()
    {
        GameObject[] items = RandomizeItems();
        GameObject[] instantiatedItems = SpawnItems(chest, items);
        LaunchItems(instantiatedItems);
    }

    void ChestManager(){
        if (chests.Length > 0)
        {
            chest = FindClosestChest();
            GameObject chestHatch = chest.transform.Find("Chest_Hatch").gameObject;
            chestAnim = chestHatch.GetComponent<Animator>();

            PlayerChest(chestAnim);
        }
    }
}

