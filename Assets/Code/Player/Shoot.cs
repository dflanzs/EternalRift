
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


public class Shoot : MonoBehaviour
{
    [SerializeField] private Player_Move _player;
    [SerializeField] private GameObject _gun, _bulletPrefab;

    [Header("Gun values")]
    
    [SerializeField] private Weapon weapon;
    public Weapon Weapon{ set {weapon = value;} }

    private float cooldownCounter = 0.0f;

    public float CooldownCounter{ set {cooldownCounter = value;} }

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
                Vector3 directionVector = _gun.transform.right * direction * (weapon._speed + Math.Abs(_player.VelActual));
                Vector3 originVector = _gun.transform.position;


                bulletScript.shoot(directionVector,originVector,weapon._range,weapon._damage);
                bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
                cooldownCounter = weapon._cooldown;
            }

        }
    }
}
