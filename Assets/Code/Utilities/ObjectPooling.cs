using System.Collections.Generic;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> bulletsPool, enemyBulletsPool;
    public Dictionary<GameObject, Vector2> enemiesPool;

    [SerializeField] private GameObject bulletPrefab, enemy1Prefab, enemy2Prefab, enemyBullet;
    
    public int bulletsPoolSize;
    public int enemiesPoolSize;
    public int enemyBulletsPoolSize;

    private static ObjectPooling poolInstance;

    public static ObjectPooling Instance
    {
        get { return poolInstance; }
    }

    void Awake()
    {   
        bulletsPool = new Queue<GameObject>();
        enemiesPool = new Dictionary<GameObject, Vector2>();     
        enemyBulletsPool = new Queue<GameObject>();
        
        if(poolInstance == null)
        {
            poolInstance = this;
        }
        else
        {
            Destroy(this);
        }   
        
        addToPool(bulletsPoolSize, enemiesPoolSize, enemyBulletsPoolSize);
    }

    private GameObject WhichEnemy(){
        int selector = Random.Range(-1, 1);
        
        if (selector > 0){
            return enemy1Prefab;
        }
        else
        {
            return enemy2Prefab;
        }
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

        for (int i = 0; i < enemiesPoolSize / 2; i++)
        {
            GameObject instantiatedPrefab = Instantiate(enemy1Prefab);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos al diccionario con su posición inicial
            enemiesPool.Add(instantiatedPrefab, instantiatedPrefab.transform.position);

            instantiatedPrefab = Instantiate(enemy2Prefab);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos al diccionario con su posición inicial
            enemiesPool.Add(instantiatedPrefab, instantiatedPrefab.transform.position);
        }

        for (int i = 0; i < enemyBulletsPoolSize; i++)
        {
            GameObject instantiatedPrefab = Instantiate(enemyBullet);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            enemyBulletsPool.Enqueue(instantiatedPrefab);
        }
    }

    public GameObject requestInstance(string objectType)
    {
        GameObject auxGO;
        if (objectType == "Bullet")
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
        else if (objectType == "Enemy")
        {
            foreach (var enemy in enemiesPool)
            {
                if (!enemy.Key.activeSelf)
                {
                    auxGO = enemy.Key;
                    return auxGO;
                }
            }
            return null;
        }
        else if (objectType == "enemyBullet")
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
