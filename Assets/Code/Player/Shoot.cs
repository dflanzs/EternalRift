
using Unity.Mathematics;
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

    private PlayerInputSystem playerInputSystem;
    private bool isShooting = false;

    void Awake()
    {
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Player.Shoot.performed += ctx => StartShooting();
        playerInputSystem.Player.Shoot.canceled += ctx => StopShooting();
    }

    private void Update()
    {
        if (isShooting)
        {
            ShootBullet();
        }

        if (cooldownCounter > 0.0f)
        {
            cooldownCounter -= Time.deltaTime;
        }
    }

    void OnEnable()
    {
        playerInputSystem.Enable();
    }

    void OnDisable()
    {
        playerInputSystem.Disable();
    }

    private void StartShooting()
    {
        isShooting = true;
    }

    private void StopShooting()
    {
        isShooting = false;
    }

    private void ShootBullet()
    {

        if (cooldownCounter <= 0.0f)
        {
            GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");

            if (bullet != null)
            {
                bullet.SetActive(true);

                bullet.transform.position = _gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Bullet bulletScript = bullet.GetComponent<Bullet>();

                int direction = _player.FacingRight ? 1 : -1;
                Vector3 directionVector = _gun.transform.right * direction;
                Vector3 originVector = _gun.transform.position;


                bulletScript.shoot(directionVector, originVector, _range, _damage);
                bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
                cooldownCounter = _cooldown;
            }

        }
    }
}
