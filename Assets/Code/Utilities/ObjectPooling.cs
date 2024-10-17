using System.Collections.Generic;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> bulletsPool, enemiesPool;

    [SerializeField] private GameObject bulletPrefab, enemy1Prefab, enemy2Prefab;
    
    public int buleetsPoolSize;
    public int enemiesPoolSize;
    public Vector3 position;

    private static ObjectPooling poolInstance;

    public static ObjectPooling Instance
    {
        get { return poolInstance; }
    }

    void Awake()
    {   
        bulletsPool = new Queue<GameObject>();
        enemiesPool = new Queue<GameObject>();        

        
        if(poolInstance == null)
        {
            poolInstance = this;
        }
        else
        {
            Destroy(this);
        }   
        
        addToPool(buleetsPoolSize, enemiesPoolSize);
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

    private void addToPool(int buleetsPoolSize, int enemiesPoolSize)
    {
       // Instanciamos cada prefab y los guardamos en la poolç
        for (int i = 0; i < buleetsPoolSize; i++)
        {
            GameObject instantiatedPrefab = Instantiate(bulletPrefab);
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            bulletsPool.Enqueue(instantiatedPrefab);
        }

        for (int i = 0; i < enemiesPoolSize; i++)
        {
            GameObject instantiatedPrefab = Instantiate(WhichEnemy());
            instantiatedPrefab.SetActive(false);

            // Metemos los objetos a la lista
            enemiesPool.Enqueue(instantiatedPrefab);
        }
    }

    public GameObject requestInstance(string objectType)
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
        else if (objectType == "Enemy")
        {
            for (int i = 0; i < enemiesPool.Count; i++)
            {
                if (!enemiesPool.Peek().activeSelf)
                {
                    auxGO = enemiesPool.Dequeue();
                    enemiesPool.Enqueue(auxGO);
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
