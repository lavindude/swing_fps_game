using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Camera playerCam;
    public float maxDistance;
    public List<Item> inventory = new List<Item>();
    public List<Gun> guns = new List<Gun>();

    public WeaponHandler weaponHandler;

    void Update()
    {
        Detect();
    }

    void Detect()
    {
        if (Input.GetKey(KeyCode.F))
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxDistance))
            {
                if (hit.collider.gameObject.GetComponent<Item>().type == "Gun")
                {
                    PickupItem(hit.collider.gameObject);
                    PickupGun(hit.collider.gameObject.GetComponent<Item>());
                }
            }
        }
    }

    void PickupItem(GameObject item)
    {
        inventory.Add(item.GetComponent<Item>());
        item.GetComponent<Item>().PickedUp();
    }

    void PickupGun(Item item)
    {
        if (weaponHandler.guns.Count < 2)
        {
            weaponHandler.guns.Add(guns[item.gunNum]);
        }
        else
        {
            weaponHandler.guns.Insert(weaponHandler.currentGunNum, guns[item.gunNum]);
            weaponHandler.guns.RemoveAt(weaponHandler.currentGunNum + 1);
            weaponHandler.UpdateGuns();
        }

    }
}
