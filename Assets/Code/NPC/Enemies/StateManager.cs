using System;
using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour
{
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

    private int hashCode;
    public DeactivatedNPCns.DeactivateNCP.DeactivatedNPCclass deactivatedNPC;
    public GameObject Crystal { get { return _crystal;} }
    private RaycastHit2D[] attackRC;
    private LayerMask attackableLayer;
    private int direction = -1;
    private bool _focused = false, _grounded = false, _found = false;
    private readonly float k_GroundedRadius = 0.2f;
    private Vector2 _startingPosition;
    private int _prevBulletHash;


    //daño visual
    public Color damageColor = Color.red;
    private SpriteRenderer spriteRenderer;
    private Color color_original;
    public float damageEffectDuration = 0.5f;  // Duración del parpadeo
    #endregion

    #region Animations
    private SpriteAnimator spriteAnimator;
    public SpriteAnimator SpriteAnimator {get { return spriteAnimator; } }
    [SerializeField] private new Animation animation;
    #endregion

    #region State Functions
    void Start()
    {
        // Store starting position for object pooling
        _startingPosition = transform.position;
        hashCode = gameObject.GetHashCode();
        
        currentState = moveState;
        prevState = null;
        currentState.EnterState(this, _player);

        attackableLayer = LayerMask.GetMask("Player");

        // Load animations
        spriteAnimator = GetComponent<SpriteAnimator>();
        animation = GetComponent<Animation>();
        
        if(_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color_original = spriteRenderer.color;  // Guarda el color original
    }


    private void OnEnable() {
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
            
            for(int i = 0; i < collidersNPC.Length; i++)
            {
                if(collidersNPC[i].gameObject.CompareTag("Bullet") 
                    && _prevBulletHash != collidersNPC[i].gameObject.GetHashCode())
                {
                    _prevBulletHash = collidersNPC[i].gameObject.GetHashCode();
                    StartCoroutine(FlashDamageEffect());
                    health -= (int) collidersNPC[i].gameObject.GetComponent<Bullet>().Damage;
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
        for(int i = 0;i < collidersGC.Length && !_grounded;i++)
        {
            if(collidersGC[i].gameObject.CompareTag("Platform"))
            {
                setGrounded(true);
                return true;
            }
        }

        setGrounded(false);
        return false;
    }

    public bool checkCollsisionEnemy()
    {
        Collider2D[] colliderLeft = Physics2D.OverlapCircleAll(_playerCollisionCheckerLeft.position, k_GroundedRadius);
        Collider2D[] colliderRight = Physics2D.OverlapCircleAll(_playerCollisionCheckerRight.position, k_GroundedRadius);

        bool enemyCollsion = false;

        for(int i = 0; i < colliderLeft.Length && !enemyCollsion;  i++)
        {
            if(colliderLeft[i].gameObject.CompareTag("npc"))
            {
                enemyCollsion = true;
            }
        }
        
        for(int i = 0; i < colliderRight.Length && !enemyCollsion;  i++)
        {
            if(colliderRight[i].gameObject.CompareTag("npc"))
                enemyCollsion = true;
        }

        return enemyCollsion;
    }

    public bool checkFocus(Transform _fieldOfView)
    {
        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_fieldOfView.position, _fieldOfView.gameObject.GetComponent<CircleCollider2D>().radius);
    
        for(int i = 0; i < collidersFOV.Length && !_focused; i++)
        {
            if(collidersFOV[i].gameObject.CompareTag("Player"))
            {
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

        for(int i = 0; i < colliderLeft.Length && !playerCollision;  i++)
        {
            if(colliderLeft[i].gameObject.CompareTag("Player"))
            {
                playerCollision = true;
            }
        }
        
        for(int i = 0; i < colliderRight.Length && !playerCollision;  i++)
        {
            if(colliderRight[i].gameObject.CompareTag("Player"))
                playerCollision = true;
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
        GameObject enemyBullet = ObjectPooling.Instance.requestInstance("enemyBullet", 0);

        if (enemyBullet != null)
        {
            //Debug.Log("Bullet");
            enemyBullet.SetActive(true);

            enemyBullet.transform.position = _gun.transform.position;
            enemyBullet.transform.rotation = Quaternion.identity;

            Bullet bulletScript = enemyBullet.GetComponent<Bullet>();
            bulletScript.setWhoShot(false);
            bulletScript.setAnimation("bulletAnimation", -1);
            
            Vector3 directionVector = getBulletSpeed() * getTarget(player, npc);
            Vector3 originVector = _gun.transform.position;


            bulletScript.shoot(directionVector, originVector, npc.getShootRange(), npc.getDamage());

            enemyBullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        }   
    }

    #endregion
    
    #region Animation playing
    public void attack()
    {
        bool _hit = false;
        if (animation.IsPlaying("attackAnimation"))
            animation.Stop();
        
        attackRC = Physics2D.CircleCastAll(getGun().transform.position, getShootRange(), getGun().transform.position, attackableLayer);

        for (int i = 0; i < attackRC.Length && !_hit; i++)
        {
            if (attackRC[i].collider.gameObject.GetComponent<Player>() != null)
            {
                animation.Play("attackAnimation");
                _hit = true;
            }
        }
    }
    public void walk()
    {
        //animation.Stop();
        spriteAnimator.Play("walkAnimation", true);
        //animation.Play("walkAnimation");
    }

    public void idle()
    {
        //animation.Stop();
        spriteAnimator.Play("idleAnimation", true);
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
    public void setPlayer(GameObject player){
        if (player.CompareTag("Player"))
        {
            this._player = player;
        }
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
        public int hashCode;
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
            hashCode = this.hashCode
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
        this.hashCode = characteristics.hashCode;
    }
    #endregion

    private IEnumerator FlashDamageEffect()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageEffectDuration);
        spriteRenderer.color = color_original;
    }
}
