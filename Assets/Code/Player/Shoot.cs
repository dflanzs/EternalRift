
using Unity.Mathematics;
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject _gun, _bulletPrefab;

    [Header("Gun values")]
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private float _range = 10.0f;
    [SerializeField] private float _damage = 5.0f;

    private float cooldownCounter = 0.0f;

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

        if (cooldownCounter <= 0.0f)
        {
            GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");

            if (bullet != null)
            {
                bullet.SetActive(true);

                bullet.transform.position = _gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Bullet bulletScript = bullet.GetComponent<Bullet>();

                int direction = player.FacingDirection;
                Vector2 directionVector = new Vector2(direction, 0);
                Vector2 originVector = new Vector2(_gun.transform.position.x, _gun.transform.position.y);


                bulletScript.shoot(directionVector, originVector, _range, _damage);
                bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
                cooldownCounter = _cooldown;
            }

        }
    }
}
