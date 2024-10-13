
using Unity.Mathematics;
using UnityEngine;


public class Shoot : MonoBehaviour
{
    public GameObject gun,bulletPrefab;
    public float damage;
    public float range = 3f;
    public float speed = 10f;
    void Update()
    {
    
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {   
                     
        GameObject bullet = Instantiate(bulletPrefab, gun.transform.position,quaternion.identity);
        Destroy(bullet,range);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = gun.transform.right;  
        rb.velocity = direction * speed ;
        }
    }
}
