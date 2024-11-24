using UnityEngine;
public class Bullet : MonoBehaviour
{
    private float _range;
    [SerializeField] private float k_GroundedRadius= 2f;
    private float _damage;
    public float Damage { get { return _damage; } }

    private Vector3 _originVector;

    [SerializeField] private Rigidbody2D _rigid;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (checkWallCollision())
        {
            Debug.Log("checkWallCollision(): true");
            gameObject.SetActive(false);
            _rigid.velocity = Vector2.zero;
        }
        else
        {
            Debug.Log("checkWallCollision(): false");

            if(_originVector == null)
                return;

            Vector2 dist = transform.position - _originVector;
            
            // Si la bala ha recorrido mas distancia que el rango de la bala la sacamos de la pool
            if(dist.magnitude >= _range){
                gameObject.SetActive(false);
                _rigid.velocity = Vector2.zero;
            }
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
    private bool checkWallCollision()
    {
        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(transform.position, k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0; i < collidersGC.Length; i++)
        {
            if(collidersGC[i].gameObject.CompareTag("Platform"))
                return true;
        }

        return false;
    }
}