using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Grapple grapple;

    public WeaponHandler weaponHandler;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI grappleText;

    public Sprite flagEnabled;
    public Sprite flagDisabled;
    public List<Image> flagImages;

    public List<Sprite> gunImages;
    public Sprite emptyGunImage;
    public Image gun1Image;
    public Image gun2Image;
    public int gunsHeld;

    public GameObject muzzleFlash;

    public AudioSource shoot;
    public AudioSource reload;

    void Update()
    {
        Detect();
        Display();
        gunsHeld = weaponHandler.guns.Count;
        UpdateGunImages();
    }

    private void Awake()
    {
        for (int i = 0; i < flagImages.Count; i++)
        {
            flagImages[i].enabled = false;
        }

        gunsHeld = 0;
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
                else if (hit.collider.gameObject.GetComponent<Item>().type == "Grapple")
                {
                    grapple.grapplesLeft++;
                    hit.collider.gameObject.GetComponent<Item>().PickedUp();
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
            APIHelper.TakeFlag(Constants.playerId, Constants.lobbyId, item.flagNum);
            inventory.Add(flags[item.flagNum - 1]);
            //item.gameObject.GetComponent<Item>().PickedUp(); *don't need this, it's set to inactive in FlagSpawn
            ShowFlag();
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
                man.weaponHandler = weaponHandler;
                man.shoot = shoot;
                man.reload = reload;
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
                auto.weaponHandler = weaponHandler;
                auto.shoot = shoot;
                auto.reload = reload;
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

    void Display()
    {
        if (weaponHandler.guns.Count > 0)
        {
            ammoText.text = weaponHandler.currentGun.ammoAmount + " / " + weaponHandler.currentGun.maxAmmo;
        }
        else
        {
            ammoText.text = "";
        }

        grappleText.text = grapple.grapplesLeft.ToString();
    }

    void ShowFlag()
    {
        flagImages[inventory.Count - 1].enabled = true;
    }

    public void resetFlagImages()
    {
        for (int i = 0; i < flagImages.Count; i++)
        {
            flagImages[i].enabled = false;
        }
    }

    void UpdateGunImages()
    {
        if (gunsHeld == 2)
        {
            gun1Image.sprite = gunImages[weaponHandler.guns[0].gunId];
            gun2Image.sprite = gunImages[weaponHandler.guns[1].gunId];
        }
        else if (gunsHeld == 1)
        {
            gun1Image.sprite = gunImages[weaponHandler.guns[0].gunId];
            gun2Image.sprite = emptyGunImage;
        }
        else
        {
            gun1Image.sprite = emptyGunImage;
            gun2Image.sprite = emptyGunImage;
        }
    }
}
