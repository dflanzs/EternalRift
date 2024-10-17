using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    private float _range;

    private float _damage;
    public float Damage { get { return _damage; } }

    private Vector3 _originVector;

    [SerializeField] private Rigidbody2D _rigid;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update(){
        Vector2 dist = transform.position - _originVector;
        
        // Si la bala ha recorrido mas distancia que el rango de la bala la sacamos de la pool
        if(dist.magnitude >= _range){
            gameObject.SetActive(false);
            _rigid.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Bullet"))
        {
            collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _rigid.velocity = Vector2.zero;
        }
    }

    public void shoot(Vector3 originVector, BoxCollider2D collision,float range,float damage)
    {
        _range = range;
        _damage = damage;
        _originVector = originVector;

        _rigid.velocity = originVector * _speed;
        collision.gameObject.SetActive(true);
    }
}