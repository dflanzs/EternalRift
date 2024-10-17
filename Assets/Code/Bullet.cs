using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
public class Bullet : MonoBehaviour
{

    public float speed = 10f;

    public Vector3 targetVector;

    public Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Bullet"))
        {
            collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void shoot(Vector3 targetVector, BoxCollider2D collision)
    {
        rigid.velocity = targetVector * speed;
        collision.gameObject.SetActive(true);
    }
}