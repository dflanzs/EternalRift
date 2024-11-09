using System.Collections;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currentState;
    public IdleState idleState = new IdleState();    
    public FocusedState focusedState = new FocusedState(); 
    public DeadState deadState = new DeadState();
    public MoveState moveState = new MoveState();

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _groundChecker; // Position of the player "feet", add a gameobject
    [SerializeField] private Transform _filedOfView; // Position of the player "feet", add a gameobject
    [SerializeField] private bool flies;
    [SerializeField] private int health;

    private BaseState prevState;
    private int direction = -1;
    private bool _focused = false;


    void Start()
    {
        // Start in MoveState by default
        currentState = moveState;
        prevState = null;
        currentState.EnterState(this, _player);
    }

    void Update()
    {
        Collider2D[] collidersNPC = Physics2D.OverlapCircleAll(transform.position, 1);
        for(int i = 0; i < collidersNPC.Length; i++){
            if(collidersNPC[i].gameObject.CompareTag("Bullet")){
                health -= 5;
            }
        }

        if (health > 0)
            currentState.UpdateState(this, _player, _groundChecker, _filedOfView);
        
        else if (health == 0)
            SwitchState(deadState);
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
    public int getDirection(){
        return direction;
    }
    public void setDirection(int direction){
        this.direction = direction;
    }
    public bool getFocus(){
        return _focused;
    }
    public void setFocus(bool focused){
        _focused = focused;
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

/*     public bool getGrounded(){
        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position, k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < collidersGC.Length && !_grounded;i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                _grounded = true;
            }
        }

        return _grounded;
    } */

}
