using UnityEngine;
public class Bullet : MonoBehaviour
{
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
        if(_originVector == null)
            return;

        Vector2 dist = transform.position - _originVector;
        
        // Si la bala ha recorrido mas distancia que el rango de la bala la sacamos de la pool
        if(dist.magnitude >= _range){
            gameObject.SetActive(false);
            _rigid.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!collider.gameObject.CompareTag("Player") && !collider.gameObject.CompareTag("fieldOfView") )
        {
            //collider.gameObject.SetActive(false); Cuando metamos a los enemigos comprobamos si es un enemigo
            _rigid.velocity = Vector2.zero;
            
            if(!collider.gameObject.CompareTag("npc"))
                gameObject.SetActive(false);
        }
    }

    public void shoot(Vector3 movement,Vector3 originVector,float range,float damage)
    {
        _range = range;
        _damage = damage;
        _originVector = originVector;

        _rigid.velocity = movement;
    }
}