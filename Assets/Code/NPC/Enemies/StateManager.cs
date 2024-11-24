using UnityEngine;

public class StateManager : MonoBehaviour
{
    public BaseState currentState;
    public IdleState idleState = new IdleState();    
    public FocusedState focusedState = new FocusedState(); 
    public DeadState deadState = new DeadState();
    public MoveState moveState = new MoveState();
    public AttackState attackState = new AttackState();

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _groundChecker; // Position of the player "feet", add a gameobject
    [SerializeField] private Transform _playerCollisionCheckerRight; // Position of the player "feet", add a gameobject
    [SerializeField] private Transform _playerCollisionCheckerLeft; // Position of the player "feet", add a gameobject
    [SerializeField] private Transform _fieldOfView; // Position of the player "feet", add a gameobject
    [SerializeField] private GameObject _gun;
    [SerializeField] private bool flies;
    [SerializeField] private int health;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shootRange = 10f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _shootCooldown = 0.5f;


    private BaseState prevState;
    private int direction = -1;
    private bool _focused = false, _grounded = false;
    private readonly float k_GroundedRadius = 0.2f;


    void Start()
    {
        // Start in MoveState by default
        currentState = moveState;
        prevState = null;
        currentState.EnterState(this, _player);

        if(_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Collider2D[] collidersNPC = Physics2D.OverlapCircleAll(transform.position, 1);

        for(int i = 0; i < collidersNPC.Length; i++){
            if(collidersNPC[i].gameObject.CompareTag("Bullet")){
                health -= (int) collidersNPC[i].gameObject.GetComponent<Bullet>().Damage;
                collidersNPC[i].gameObject.SetActive(false);
            }
        }

        if (health > 0)
            currentState.UpdateState(this, _player, _groundChecker, _fieldOfView);
        
        else if (health <= 0)
            SwitchState(deadState);
    }

    // Checkers
    public bool checkGrounded(Transform _groundChecker)
    {
        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position, k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < collidersGC.Length && !_grounded;i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                setGrounded(true);
                return true;
            }
        }

        setGrounded(false);
        return false;
    }
    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this, _player);
    }

    public bool checkFocus(Transform _fieldOfView)
    {
        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_fieldOfView.position, _fieldOfView.gameObject.GetComponent<CircleCollider2D>().radius);
    
        for(int i = 0; i < collidersFOV.Length && !_focused; i++){
            
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                setFocus(true);
                return true;
            }
        }

        setFocus(false);
        return false;
    }
    
    public bool checkPlayerCollision()
    {
        Collider2D[] colliderLeft = Physics2D.OverlapCircleAll(_playerCollisionCheckerLeft.position, k_GroundedRadius);
        Collider2D[] colliderRight = Physics2D.OverlapCircleAll(_playerCollisionCheckerRight.position, k_GroundedRadius);
        
        bool playerCollision = false;

        for(int i = 0; i < colliderLeft.Length && !playerCollision;  i++){
            if(colliderLeft[i].gameObject.CompareTag("Player")){
                playerCollision = true;
            }
        }
        
        for(int i = 0; i < colliderRight.Length && !playerCollision;  i++){
            if(colliderRight[i].gameObject.CompareTag("Player")){
                playerCollision = true;
            }
        }

        return playerCollision;
    }

    public void ShootBullet(StateManager npc, GameObject player)
    {
        Debug.Log("Shoot");
        GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");

        if (bullet != null)
        {
            Debug.Log("Bullet");
            bullet.SetActive(true);

            bullet.transform.position = _gun.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Bullet bulletScript = bullet.GetComponent<Bullet>();

            Vector3 directionVector = getBulletSpeed() * getTarget(player, npc);
            Vector3 originVector = _gun.transform.position;


            bulletScript.shoot(directionVector,originVector,npc.getShootRange(), npc.getDamage());

            bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        }   
    }

    // Getters and setters
    public bool getFlies(){
        return flies;
    }

    public bool getGrounded(){
        return _grounded;
    }
    public void setGrounded(bool focused){
        _grounded = focused;
    }

    public bool getFocus(){
        return _focused;
    }
    public void setFocus(bool focused){
        _focused = focused;
    }

    public int getDirection(){
        return direction;
    }
    public void setDirection(int direction){
        this.direction = direction;
    }

    public int getHealth(){
        return health;
    }
    public void setHealth(int health){
        this.health = health;
    }

    public BaseState getPrevstate(){
        return prevState;
    }
    public void setPrevstate(BaseState newState){
        prevState = newState;
    }

    // Shooting
    public float getBulletSpeed()
    {
        return _bulletSpeed;
    }
    public void setBulletSpeed(float bulletSpeed)
    {
        _bulletSpeed = bulletSpeed;
    }

    public float getShootRange()
    {
        return _shootRange;
    }
    public void setShootRange(float shootRange)
    {
        _shootRange = shootRange;
    }

    public float getDamage()
    {
        return _damage;
    }
    public void setDamage(float damage)
    {
        _damage = damage;
    }

    public float getShootCooldown()
    {
        return _shootCooldown;
    }
    public void setShootCooldown(float shootCooldown)
    {
        _shootCooldown = shootCooldown;
    }

    public Vector2 getTarget(GameObject player, StateManager npc){
        return (player.transform.position - npc.gameObject.transform.position).normalized;
    }
}
