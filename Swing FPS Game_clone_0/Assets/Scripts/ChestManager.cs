using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestManager : MonoBehaviour
{
    public TextMeshProUGUI chestOpenText;
    public TextMeshProUGUI chestCloseText;

    public GameObject railwayChests;
    public GameObject cityChests;
    public GameObject slumChests;
    public GameObject zenChests;
    public GameObject parkChests;

    private int maxRailway = 10;
    private int maxCity = 12;
    private int maxSlum = 13;
    private int maxZen = 7;
    private int maxPark = 8;

    public GameObject[] chestItems;
    private GameObject[] chests;
    private GameObject chest;
    private Animator chestAnim;
    // Start is called before the first frame update
    void Start()
    {
        RandomizeChestLocations();
    }

    // Update is called once per frame
    void Update()
    {
        ChestOpener();
    }

    void RandomizeChestLocations()
    {
        int currChest;

        for (int i = 0; i < maxRailway; i++)
        {
            currChest = Random.Range(0, railwayChests.transform.childCount);

            if(railwayChests.transform.GetChild(currChest).gameObject.activeSelf)
            {
                i--;
            }
            else
            {
                railwayChests.transform.GetChild(currChest).gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < maxCity; i++)
        {
            currChest = Random.Range(0, cityChests.transform.childCount);

            if(cityChests.transform.GetChild(currChest).gameObject.activeSelf)
            {
                i--;
            }
            else
            {
                cityChests.transform.GetChild(currChest).gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < maxSlum; i++)
        {
            currChest = Random.Range(0, slumChests.transform.childCount);

            if(slumChests.transform.GetChild(currChest).gameObject.activeSelf)
            {
                i--;
            }
            else
            {
                slumChests.transform.GetChild(currChest).gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < maxZen; i++)
        {
            currChest = Random.Range(0, zenChests.transform.childCount);

            if(zenChests.transform.GetChild(currChest).gameObject.activeSelf)
            {
                i--;
            }
            else
            {
                zenChests.transform.GetChild(currChest).gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < maxPark; i++)
        {
            currChest = Random.Range(0, parkChests.transform.childCount);

            if(parkChests.transform.GetChild(currChest).gameObject.activeSelf)
            {
                i--;
            }
            else
            {
                parkChests.transform.GetChild(currChest).gameObject.SetActive(true);
            }
        }

        chests = GameObject.FindGameObjectsWithTag("Chest");
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

        Vector3 spawnPos = new Vector3(spawnChest.transform.position.x, spawnChest.transform.position.y + 0.5f, 
            spawnChest.transform.position.z);

        for(int i = 0; i < chestItems.Length; i++)
        {
            instantiatedItems[i] = Instantiate(chestItems[i], spawnPos, 
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
            itemBodies[i].AddForce(transform.up * 8.0f, ForceMode.Impulse);
            //itemBodies[i].AddForce(transform.right * 7.5f, ForceMode.Impulse);
        }
    }

    GameObject FindClosestChest()
    {
        if(chests.Length > 1)
        {
            int index = new int();
            float prevDist = Vector3.Distance(transform.position, chests[0].transform.position);
            float currDist;
            for(int i = 1; i < chests.Length; i++)
            {
                currDist = Vector3.Distance(transform.position, chests[i].transform.position);
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
        if (Vector3.Distance(transform.position, chest.transform.position) < 3)
        {
            if (GetChestState(chestAnim))
            {
                SetOpenTextTrue();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SetOpenTextFalse();
                    StartCoroutine(Chest(chestAnim));
                }
            }
        }
        else
        {
            SetChestTextFalse();
        }
    }

    void ItemLauncher()
    {
        GameObject[] items = RandomizeItems();
        GameObject[] instantiatedItems = SpawnItems(chest, items);
        LaunchItems(instantiatedItems);
    }

    void ChestOpener(){
        if (chests.Length > 0)
        {
            chest = FindClosestChest();
            GameObject chestHatch = chest.transform.Find("Chest_Hatch").gameObject;
            chestAnim = chestHatch.GetComponent<Animator>();

            PlayerChest(chestAnim);
        }
    }

    IEnumerator Chest(Animator chestAnim)
    {
        ChestOpen(chestAnim);
        yield return new WaitForSeconds(1.5f);
        ItemLauncher();
    }

    public void SetOpenTextTrue()
    {
        chestOpenText.gameObject.SetActive(true);
    }

    public void SetOpenTextFalse()
    {
        chestOpenText.gameObject.SetActive(false);
    }

    public void SetCloseTextTrue()
    {
        chestCloseText.gameObject.SetActive(true);
    }

    public void SetCloseTextFalse()
    {
        chestCloseText.gameObject.SetActive(false);
    }

    public void SetChestTextFalse()
    {
        chestOpenText.gameObject.SetActive(false);
        chestCloseText.gameObject.SetActive(false);
    }
}

