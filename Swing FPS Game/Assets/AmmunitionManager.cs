using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionManager : MonoBehaviour
{
    public static AmmunitionManager instance;

    private Dictionary<AmmunitionTypes, int> ammoCounts = new Dictionary<AmmunitionTypes, int>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(AmmunitionTypes)).Length; i++)
        {
            ammoCounts.Add((AmmunitionTypes)i, 0);
        }
    }
    public bool ConsumeAmmo(AmmunitionTypes ammunitionType)
    {
        if (ammoCounts[ammunitionType] > 0)
        {
            ammoCounts[ammunitionType]--;
            return true;
        }
        else
        {
            return false;
        }
    }

}
