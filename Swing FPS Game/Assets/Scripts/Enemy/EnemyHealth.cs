using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats enemyStats;

    private int currentHealth;

    private void Start()
    {
        currentHealth = enemyStats.maxHealth;
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DealDamage(int damage)
    {
        //send info to API that this player got shot
        for (int i = 0; i < EnemyObjectData.otherPlayerObjects.Length; i++)
        {
            if (EnemyObjectData.otherPlayerObjects[i] != null && EnemyObjectData.otherPlayerObjects[i].enemyPrefab == gameObject)
            {
                APIHelper.DealDamage(EnemyObjectData.otherPlayerObjects[i].enemyId, damage);
                break;
            }
        }
        currentHealth -= damage;
    }
}
