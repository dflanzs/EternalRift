using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject _gun, _bulletPrefab;

    [Header("Gun values")]
    [SerializeField] private Weapon weapon;
    public Weapon Weapon
    {
        get { return weapon; }
        set { weapon = value; }
    }

    private float cooldownCounter = 0.0f;
    public float CooldownCounter { set { cooldownCounter = value; } }
    private bool shootInput;

    // Rango de detección de enemigos
    [SerializeField] private float detectionRange = 10f;

    // Usar etiquetas en lugar de capas
    [SerializeField] private string enemyTag = "npc";

    private Transform nearestEnemy; // Referencia al enemigo más cercano
    
    private void Update()
    {
        // Reduce el cooldown
        if (cooldownCounter > 0.0f)
        {
            cooldownCounter -= Time.deltaTime;
        }

        // Detectar disparo manual
        shootInput = player.InputHandler.ShootInput;

        if (shootInput && cooldownCounter <= 0.0f)
        {
            ShootBullet();
            return; // Prioriza el disparo manual
        }

        // Detectar disparo automático
        if(GameManager.Instance == null)
            Debug.LogWarning("No has puesto el GameManager");

        if (GameManager.Instance != null && GameManager.Instance.autoShoot)
        {
            DetectAndShootEnemy();
        }
    }

    private void DetectAndShootEnemy()
    {
        if (cooldownCounter > 0.0f) return; // Respetar el cooldown

        nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            ShootBullet();
        }
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = collider.transform;
                }
            }
        }

        return closest;
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

                ShootDir dir = player.playerData.shootDir;
                Vector3 direction;
                 
                if(dir == ShootDir.UP){
                    direction = _gun.transform.up;
                    bullet.transform.Rotate(new Vector3(0,0,90));
                } else
                    direction = _gun.transform.right;

                Vector3 directionVector = direction * (weapon._speed + Math.Abs(player.CurrentVelocity.magnitude));
                Vector3 originVector = _gun.transform.position;
                bulletScript.setWhoShot(true); // true si es el jugador, false si es un NPC
                
                if (weapon.name == "Sniper"){
                    bulletScript.ShotByShotgun(false);
                    bulletScript.setAnimation("rifleAnimation", directionVector.x);
                }
                else if(weapon.name == "Shootgun"){
                    bulletScript.ShotByShotgun(true);
                    bulletScript.setAnimation("shotgunAnimation", directionVector.x);
                }
                
                bulletScript.shoot(directionVector, originVector, weapon._range, weapon._damage);

                bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
                if(!player.playerData.cooldownWeapons)
                    cooldownCounter = weapon._cooldown;
                else
                    cooldownCounter = weapon._cooldown * player.playerData.cooldownFactor;

                //Debug.Log($"Disparo realizado. Disparo Automático: {autoShootEnabled}");
            }
        }
    }

}
