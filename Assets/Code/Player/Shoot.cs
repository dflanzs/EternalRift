using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject _gun, _bulletPrefab;

    [Header("Gun values")]
    
    [SerializeField] private Weapon weapon;
    public Weapon Weapon{ set {weapon = value;} }

    private float cooldownCounter = 0.0f;

    public float CooldownCounter{ set {cooldownCounter = value;} }
    private bool shootInput;

    private void Update()
    {

        shootInput = player.InputHandler.ShootInput;

        if (shootInput)
        {
            ShootBullet();
        }

        if (cooldownCounter > 0.0f)
        {
            cooldownCounter -= Time.deltaTime;
        }
    }

    private void ShootBullet()
    {

        if (GameManager.Instance.hasWeapon && cooldownCounter <= 0.0f)
        {
            GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet", 0);

            if (bullet != null)
            {
                bullet.SetActive(true);

                bullet.transform.position = _gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Bullet bulletScript = bullet.GetComponent<Bullet>();

                Vector3 directionVector = _gun.transform.right * (weapon._speed + Math.Abs(player.CurrentVelocity.magnitude));
                Vector3 originVector = _gun.transform.position;
                bulletScript.setWhoShot(true); // true si es el jugador, false si es un NPC

                bulletScript.shoot(directionVector,originVector,weapon._range,weapon._damage);

                bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
                cooldownCounter = weapon._cooldown;
            }
        }
    }
}

