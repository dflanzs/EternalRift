using UnityEngine;
public class Bullet : MonoBehaviour
{
    private float _range;
    [SerializeField] private float k_GroundedRadius= 2f;
    private float _damage;
    private  bool _shotByPlayer;
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
            gameObject.SetActive(false);
            _rigid.velocity = Vector2.zero;
        }
        else
        {
            if(_originVector == null)
                return;

            Vector2 dist = transform.position - _originVector;
            

            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, k_GroundedRadius);
        
            bool collision = false;

            if (_shotByPlayer)
            {    
                Debug.Log("Shot by player, must disappear");
                    
                for(int i = 0; i < collisions.Length && !collision;  i++){
                    if(collisions[i].gameObject.CompareTag("npc")){
                        collision = true;
                    }
                }
                //  Disparos por el player solo para los enemigos
                if(collision)
                {
                    gameObject.SetActive(false);
                    _rigid.velocity = Vector2.zero;
                }
            }
            else
            {
                for(int i = 0; i < collisions.Length && !collision;  i++){
                if(collisions[i].gameObject.CompareTag("Player")){
                    collision = true;
                }
            }
                if (collision)
                {
                    Debug.Log("Shot by enemy, must disappear");
                    gameObject.SetActive(false);
                    _rigid.velocity = Vector2.zero;
                }
            }
            
            // Si la bala ha recorrido mas distancia que el rango de la bala la sacamos de la pool
            if(dist.magnitude >= _range){
                gameObject.SetActive(false);
                _rigid.velocity = Vector2.zero;
            }
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

    public void setWhoShot(bool player){
        _shotByPlayer = player;
    }
}