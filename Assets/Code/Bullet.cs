using UnityEngine;
public class Bullet : MonoBehaviour
{
    private float _range;
    [SerializeField] private float k_GroundedRadius= 2f;
    private int _damage;
    private  bool _shotByPlayer;
    public int Damage { get { return _damage; } }

    private SpriteAnimator spriteAnimator;
    private new string animation;
    private Vector3 _originVector;
    private float _spriteDirection;
    private bool _shotgun, _collision;
    private PlayerHealth _playerHealth;

    [SerializeField] private Rigidbody2D _rigid;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        spriteAnimator = GetComponent<SpriteAnimator>();
        _collision = false;
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (_shotByPlayer)
            spriteAnimator.FlipTo(_spriteDirection);

        if (!spriteAnimator.IsPlaying(animation))
            spriteAnimator.Play(animation, true);
        
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
        

            if (_shotByPlayer)
            {    
                if (!_shotgun)
                {    
                    for(int i = 0; i < collisions.Length && !_collision;  i++){
                        if(collisions[i].gameObject.CompareTag("npc")){
                            _collision = true;
                            _playerHealth.TakeDamage(_damage);
                        }
                    }
                    //  Disparos por el player solo para los enemigos
                    if(_collision)
                    {
                        gameObject.SetActive(false);
                        _rigid.velocity = Vector2.zero;
                    }
                    _collision = false;
                } 
                
            }
            else
            {
                for(int i = 0; i < collisions.Length && !_collision;  i++){
                    if(collisions[i].gameObject.CompareTag("Player"))
                    {
                        _collision = true;
                    }
                }
                if (_collision)
                {
                    gameObject.SetActive(false);
                    _rigid.velocity = Vector2.zero;
                }

                _collision = false;
            }
            
            // Si la bala ha recorrido mas distancia que el rango de la bala la sacamos de la pool
            if(dist.magnitude >= _range){
                gameObject.SetActive(false);
                _rigid.velocity = Vector2.zero;
            }
        }
    }

    public void shoot(Vector3 movement,Vector3 originVector,float range,int damage)
    {
        _range = range;
        _damage = damage;
        _originVector = originVector;

        _rigid.velocity = movement;
    }
    private bool checkWallCollision()
    {
        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(transform.position, k_GroundedRadius);

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

    public void setAnimation(string animation, float direction)
    {
        this.animation = animation;
        Debug.Log("Animation set to: " + animation);
        _spriteDirection = direction;
    }

    public void ShotByShotgun(bool shotgun)
    {
        _shotgun = shotgun;
    }
}