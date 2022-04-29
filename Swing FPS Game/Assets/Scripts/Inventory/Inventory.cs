using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Camera playerCam;
    public float maxDistance;
    public List<Item> inventory = new List<Item>();
    public List<Gun> guns = new List<Gun>();
    public List<Item> flags = new List<Item>();
    public List<GameObject> prefabs = new List<GameObject>();
    public Transform dropPos;
    public int dropForce;

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
                    PickupGun(hit.collider.gameObject.GetComponent<Item>());
                }
                else if (hit.collider.gameObject.GetComponent<Item>().type == "Flag")
                {
                    PickupFlag(hit.collider.gameObject.GetComponent<Item>());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (weaponHandler.guns.Count > 0)
            {
                DropGun();
            }
        }
    }

    void PickupFlag(Item item)
    {
        bool canAdd = true;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].flagNum == item.flagNum)
            {
                canAdd = false;
            }
        }

        if (canAdd)
        {
            inventory.Add(flags[item.flagNum - 1]);
            item.gameObject.GetComponent<Item>().PickedUp();
        }
    }

    void PickupGun(Item item)
    {
        if (weaponHandler.guns.Count < 2)
        {
            Manual man;
            Automatic auto;

            if (item.gunNum == 0)
            {
                man = ScriptableObject.CreateInstance<Manual>();
                man.gunId = guns[item.gunNum].gunId;
                man.gunName = guns[item.gunNum].gunName;
                man.gunPrefab = guns[item.gunNum].gunPrefab;
                man.ammoAmount = item.ammoAmount;
                man.maxAmmo = guns[item.gunNum].maxAmmo;
                man.minDamage = guns[item.gunNum].minDamage;
                man.maxDamage = guns[item.gunNum].maxDamage;
                man.maximumRange = guns[item.gunNum].maximumRange;
                man.ImpactParticleSystem = guns[item.gunNum].ImpactParticleSystem;
                weaponHandler.guns.Add(man);
            }
            else
            {
                auto = ScriptableObject.CreateInstance<Automatic>();
                auto.gunId = guns[item.gunNum].gunId;
                auto.gunName = guns[item.gunNum].gunName;
                auto.gunPrefab = guns[item.gunNum].gunPrefab;
                auto.ammoAmount = item.ammoAmount;
                auto.maxAmmo = guns[item.gunNum].maxAmmo;
                auto.minDamage = guns[item.gunNum].minDamage;
                auto.maxDamage = guns[item.gunNum].maxDamage;
                auto.maximumRange = guns[item.gunNum].maximumRange;
                auto.ImpactParticleSystem = guns[item.gunNum].ImpactParticleSystem;
                auto.fireRate = 10;
                weaponHandler.guns.Add(auto);
            }
        }
        else
        {
            weaponHandler.guns.Insert(weaponHandler.currentGunNum, guns[item.gunNum]);
            weaponHandler.guns.RemoveAt(weaponHandler.currentGunNum + 1);
            weaponHandler.UpdateGuns();
            weaponHandler.setAmmo(item);
        }

        item.gameObject.GetComponent<Item>().PickedUp();
    }

    void DropGun()
    {
        GameObject newGun = Instantiate(prefabs[weaponHandler.currentGun.gunId]);
        newGun.transform.position = dropPos.position;
        newGun.transform.rotation = playerCam.transform.rotation;
        newGun.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * dropForce, ForceMode.Impulse);
        newGun.GetComponent<Item>().ammoAmount = weaponHandler.currentGun.ammoAmount;
        weaponHandler.guns.RemoveAt(weaponHandler.currentGunNum);
        weaponHandler.UpdateGuns();
    }
}
