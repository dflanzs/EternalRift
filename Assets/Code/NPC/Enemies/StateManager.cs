using System;
using UnityEngine;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    // To use animation go to project window, click on the animation file, open debug mode, activate legacy 

    #region States
    public BaseState currentState;
    private BaseState prevState;
    public IdleState idleState = new IdleState();    
    public MoveState moveState = new MoveState();
    public FocusedState focusedState = new FocusedState(); 
    public AttackState attackState = new AttackState();
    public DeadState deadState = new DeadState();
    public DeactivatedState deactivatedState = new DeactivatedState();
    #endregion

    #region Data
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private Transform _playerCollisionCheckerRight;
    [SerializeField] private Transform _playerCollisionCheckerLeft;
    [SerializeField] private Transform _fieldOfView;
    [SerializeField] private GameObject _gun;
    [SerializeField] private bool flies;
    [SerializeField] private int health;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shootRange = 10f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _shootCooldown = 1f;
    [SerializeField] private GameObject _crystal;

    public GameObject Crystal { get { return _crystal;} }
    private RaycastHit2D[] attackRC;
    private LayerMask attackableLayer;
    private int direction = -1;
    private bool _focused = false, _grounded = false, _found = false;
    private readonly float k_GroundedRadius = 0.2f;
    private Vector2 _startingPosition;
    #endregion

    #region State Functions
    void Start()
    {
        //Guardamos la posicion inicial para el objectPooling
        _startingPosition = transform.position;

        currentState = moveState;
        prevState = null;
        currentState.EnterState(this, _player);

        attackableLayer = LayerMask.GetMask("Player");

        if(_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!playerIsNear(transform.position))
        {
            deactivatedState.EnterState(this, _player);    
        }
        else 
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
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this, _player);
    }
    #endregion

    #region Checkers
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

    public bool playerIsNear(Vector2 _lastPosition)
    {
        if (Math.Abs(_player.transform.position.x - _lastPosition.x) >= 30 ||
            Math.Abs(_player.transform.position.y - _lastPosition.y) >= 30)
        {
            return false;
        }
        return true;
    }
    #endregion

    #region Auxiliar Functions
    public void ShootBullet(StateManager npc, GameObject player)
    {
        //Debug.Log("Shoot");
        GameObject enemyBullet = ObjectPooling.Instance.requestInstance("enemyBullet");

        if (enemyBullet != null)
        {
            //Debug.Log("Bullet");
            enemyBullet.SetActive(true);

            enemyBullet.transform.position = _gun.transform.position;
            enemyBullet.transform.rotation = Quaternion.identity;

            Bullet bulletScript = enemyBullet.GetComponent<Bullet>();
            bulletScript.setWhoShot(false);
            
            Vector3 directionVector = getBulletSpeed() * getTarget(player, npc);
            Vector3 originVector = _gun.transform.position;


            bulletScript.shoot(directionVector, originVector, npc.getShootRange(), npc.getDamage());

            enemyBullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        }   
    }

    public void attack(StateManager npc, Player player)
    {
        Animation animation = npc.gameObject.GetComponent<Animation>();
        animation["Enemy1"].layer = 0;

        bool _hit = false;
        attackRC = Physics2D.CircleCastAll(npc.getGun().transform.position, npc.getShootRange(), npc.getGun().transform.position, attackableLayer);

        for (int i = 0; i < attackRC.Length && !_hit; i++)
        {
            if (attackRC[i].collider.gameObject.GetComponent<Player>() != null)
            {
                animation.Play("Enemy1");                
                player.Health -= npc.getDamage();

                // Solo un hit hace daño
                _hit = true;
            }
        }
    }
    #endregion

    #region Getters and setters
    public bool getFlies(){
        return flies;
    }

    public void setFlies(bool _flies){
        flies = _flies;
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

    public Vector2 getTarget(GameObject player, StateManager npc){
        return (player.transform.position - npc.gameObject.transform.position).normalized;
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
    public GameObject getGun(){
        return _gun;
    }
    public bool getFound(){
        return _found;
    }
    public void setFound(bool found){
        _found = found;
    }
    public Vector2 getStartingPosition(){
        return _startingPosition;
    }

    [Serializable]
    public class NPCCharacteristics
    {
        public bool flies;
        public int health;
        public float bulletSpeed;
        public float shootRange;
        public float damage;
        public float shootCooldown;
        public GameObject crystal;
        public int direction;
        public bool focused;
        public bool grounded;
        public bool found;
        public Vector2 startingPosition;
        // Agrega otras características si es necesario
    }

    public NPCCharacteristics GetAllCharacteristics()
    {
        return new NPCCharacteristics
        {
            flies = this.getFlies(),
            health = this.getHealth(),
            bulletSpeed = this.getBulletSpeed(),
            shootRange = this.getShootRange(),
            damage = this.getDamage(),
            shootCooldown = this.getShootCooldown(),
            crystal = this._crystal,
            startingPosition = this._startingPosition,
        };
    }

    public void SetAllCharacteristics(NPCCharacteristics characteristics)
    {
        this.setFlies(characteristics.flies);
        this.setHealth( characteristics.health);
        this.setBulletSpeed(characteristics.bulletSpeed);
        this.setShootRange(characteristics.shootRange);
        this.setDamage(characteristics.damage);
        this.setShootCooldown(characteristics.shootCooldown);
        this._crystal = characteristics.crystal;
        this._startingPosition = characteristics.startingPosition;
    }
    #endregion
}
