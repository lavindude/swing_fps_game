using UnityEngine;

public static class EnemyObjectData
{
    public class EnemyObject
    {
        public int enemyId;
        public GameObject enemyPrefab;

        public EnemyObject(int id, GameObject otherPlayerPrefab)
        {
            enemyId = id;
            enemyPrefab = otherPlayerPrefab;
        }

        public void setOtherPlayerPrefab(Vector3 otherPosition)
        {
            enemyPrefab.transform.position = otherPosition;
        }
    }

    public static EnemyObject[] otherPlayerObjects;

    public static void setEnemyArrayLength(int len)
    {
        otherPlayerObjects = new EnemyObject[len];
    }

    public static void fillUpEnemyArray(GameObject newEnemy, int id, int index)
    {

        EnemyObject newEnemyObject = new EnemyObject(id, newEnemy);
        otherPlayerObjects[index] = newEnemyObject;
    }
}