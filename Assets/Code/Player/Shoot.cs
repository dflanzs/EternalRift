
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private Player_Move _player;
    [SerializeField] private GameObject _gun, _bulletPrefab;

    [Header("Gun values")]
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private float _range = 10.0f;
    [SerializeField] private float _damage = 5.0f;

    private float cooldownCounter = 0.0f;

    private void Update() {
        if(cooldownCounter > 0.0f)
            cooldownCounter -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse0) && cooldownCounter <= 0.0f)
        {   
            GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");
            
            if(bullet != null)
            {            
                bullet.SetActive(true);
                
                bullet.transform.position = _gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Bullet bulletScript = bullet.GetComponent<Bullet>();

                int direction = _player.FacingRight ? 1 : -1;
                Vector3 directionVector = _gun.transform.right * direction;
                Vector3 originVector = _gun.transform.position;


                bulletScript.shoot(directionVector,originVector,_range,_damage);
                bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
                cooldownCounter = _cooldown;
            }

        }
    }
}
