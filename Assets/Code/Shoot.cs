
using Unity.Mathematics;
using UnityEngine;


public class Shoot : MonoBehaviour
{
    public GameObject gun, bulletPrefab;
    public float range = 3;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {   
            GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");
            Debug.Log("GameObject bullet");   
            
            if(bullet != null)
            {            
                bullet.SetActive(true);
                
                bullet.transform.position = gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.shoot(gun.transform.right, bullet.GetComponent<BoxCollider2D>());
                
                Debug.Log("Disparo...");   
            }

        }
        checkBulletOutOfBounds(gun);
    }

    private void checkBulletOutOfBounds(GameObject gun)
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach(GameObject bullet in bullets)
        {
            Debug.Log("Desaparece");
            if(Mathf.Abs(bullet.transform.position.x) > gun.transform.position.x + range || 
                Mathf.Abs(bullet.transform.position.y) > gun.transform.position.y + range)
            {
                bullet.SetActive(false);
            }
        }
    }
}
