using System.Collections.Generic;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> bulletsPool = new Queue<GameObject>(), enemyBulletsPool = new Queue<GameObject>(); 
    Dictionary<int, GameObject> enemy1Pool = new Dictionary<int, GameObject>(), enemy2Pool = new Dictionary<int, GameObject>();

    [SerializeField] private GameObject bulletPrefab, enemy1Prefab, enemy2Prefab, enemyBullet;
    List<int> enemy1Positions = new List<int>();
    List<int> enemy2Positions = new List<int>();
    public int bulletsPoolSize;
    public int enemiesPoolSize;
    public int enemyBulletsPoolSize;

    private static ObjectPooling poolInstance;

    public static ObjectPooling Instance
    {
        get { return poolInstance; }
    }

    void Start()
    {   
        if(poolInstance == null)
        {
            poolInstance = this;
        }
        else
        {
            Destroy(this);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("npc");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<StateManager>().getFlies())
                enemy2Positions.Add(enemy.GetInstanceID());
            else
                enemy1Positions.Add(enemy.GetInstanceID());
        }

        addToPool(bulletsPoolSize, enemiesPoolSize, enemyBulletsPoolSize);
    }

    private void addToPool(int bulletsPoolSize, int enemiesPoolSize, int enemyBulletsPoolSize)
    {
       // Instanciamos cada prefab y los guardamos en la pool
        for (int i = 0; i < bulletsPoolSize; i++)
        {
            GameObject instantiatedPrefab = Instantiate(bulletPrefab);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            bulletsPool.Enqueue(instantiatedPrefab);
        }

        for (int i = 0; i < enemy1Positions.Count; i++)
        {
            GameObject instantiatedPrefab = Instantiate(enemy1Prefab);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            enemy1Pool.Add(enemy1Positions[i], instantiatedPrefab);
        }

        for (int i = 0; i < enemy2Positions.Count; i++)
        {
            GameObject instantiatedPrefab = Instantiate(enemy2Prefab);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            enemy2Pool.Add(enemy2Positions[i], instantiatedPrefab);
        }

        for (int i = 0; i < enemyBulletsPoolSize; i++)
        {
            GameObject instantiatedPrefab = Instantiate(enemyBullet);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            enemyBulletsPool.Enqueue(instantiatedPrefab);
        }
    }

    public GameObject requestInstance(string objectType, int instanceID)
    {
        GameObject auxGO;
        if(objectType == "Bullet")
        {
            for (int i = 0; i < bulletsPool.Count; i++)
            {
                if (!bulletsPool.Peek().activeSelf)
                {
                    auxGO = bulletsPool.Dequeue();
                    bulletsPool.Enqueue(auxGO);
                    return auxGO;
                }
            }
            return null;
        } 
        else if (objectType == "Enemy1")
        {
            if (enemy1Pool == null)
            {
                Debug.LogError("enemy1Pool is null");
                return null;
            }

            if (!enemy1Pool.TryGetValue(instanceID, out auxGO))
            {
                Debug.LogError($"No enemy1 found with instanceID: {instanceID}");
                return null;
            }

            if (auxGO == null)
            {
                Debug.LogError("auxGO is null for Enemy1");
                return null;
            }

            if (!auxGO.activeSelf)
            {
                return auxGO;
            }
            return null;
        } 
        else if (objectType == "Enemy2")
        {
            if (enemy2Pool == null)
            {
                Debug.LogError("enemy2Pool is null");
                return null;
            }

            if (!enemy2Pool.TryGetValue(instanceID, out auxGO))
            {
                Debug.LogError($"No enemy2 found with instanceID: {instanceID}");
                return null;
            }

            if (auxGO == null)
            {
                Debug.LogError("auxGO is null for Enemy2");
                return null;
            }

            if (!auxGO.activeSelf)
            {
                return auxGO;
            }
            return null;
        } 
        else if(objectType == "enemyBullet")
        {
            for (int i = 0; i < enemyBulletsPool.Count; i++)
            {
                if (!enemyBulletsPool.Peek().activeSelf)
                {
                    auxGO = enemyBulletsPool.Dequeue();
                    enemyBulletsPool.Enqueue(auxGO);
                    return auxGO;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }
}