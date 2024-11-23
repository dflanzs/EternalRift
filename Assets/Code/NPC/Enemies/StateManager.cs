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
    [SerializeField] private Transform _filedOfView; // Position of the player "feet", add a gameobject
    [SerializeField] private bool flies;
    [SerializeField] private int health;

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
            currentState.UpdateState(this, _player, _groundChecker, _filedOfView);
        
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
        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position, k_GroundedRadius);
        bool playerCollision = false;

        for(int i = 0; i < collidersGC.Length && !playerCollision;  i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                playerCollision = true;
            }
        }

        return playerCollision;
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this, _player);
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
}
