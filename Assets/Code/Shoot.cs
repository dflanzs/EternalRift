
using Unity.Mathematics;
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _gun, _bulletPrefab;
    [SerializeField] private float _range = 10.0f;
    [SerializeField] private float _damage = 5.0f;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {   
            GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");
            
            if(bullet != null)
            {            
                bullet.SetActive(true);
                
                bullet.transform.position = _gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.shoot(_gun.transform.right, bullet.GetComponent<BoxCollider2D>(),_range,_damage);
                 
            }

        }
    }
}
