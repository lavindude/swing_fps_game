using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public List<Gun> guns = new List<Gun>();

    public Gun currentGun;
    private Transform cameraTransform;
    private GameObject currentGunPrefab;
    public GameObject muzzleFlash;
    public int currentGunNum;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        currentGunPrefab = null;
        currentGun = null;
    }

    private void Update()
    {
        if (guns.Count > 0)
        {
            CheckForShooting();
            Reload();
        }

        if (guns.Count > 0 && currentGunPrefab == null)
        {
            currentGunPrefab = Instantiate(guns[0].gunPrefab, transform);
            currentGun = guns[0];
            currentGunNum = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && guns[0] != null)
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(guns[0].gunPrefab, transform);
            currentGun = guns[0];
            currentGunNum = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && guns[1] != null)
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(guns[1].gunPrefab, transform);
            currentGun = guns[1];
            currentGunNum = 1;
        }

        muzzleFlash = currentGunPrefab.GetComponent<GunValues>().muzzleFlash;
    }

    private void CheckForShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentGun.OnLeftMouseDown(cameraTransform);
        }
        if (Input.GetMouseButton(0))
        {
            currentGun.OnLeftMouseHold(cameraTransform);
        }
        if (Input.GetMouseButtonDown(1))
        {
            currentGun.OnRightMouseDown();
        }

    }

    public void UpdateGuns()
    {
        Destroy(currentGunPrefab);
        currentGunPrefab = Instantiate(guns[currentGunNum].gunPrefab, transform);
        currentGun = guns[currentGunNum];
    }

    public void setAmmo(Item item)
    {
        currentGun.ammoAmount = item.ammoAmount;
    }

    public void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun.ammoAmount != currentGun.maxAmmo)
            {
                currentGun.ammoAmount = currentGun.maxAmmo;
                currentGun.reload.Play();
            }
        }
    }
}
