using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class Player_Move : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocidad = 7f; //ajustar velocidad según veamos
    private float velActual = 0f;// la velocidad actual del objeto, la necesitamos porque no será constante
    public float maxVelocidad = 15f;//maxima velocidad permitida
    public float aceleracion = 5f;//la aceleracion para crear un movimiento más fluido y no sea muy rigido
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        manejoMovimiento();


    }
    private void manejoMovimiento()
    {
       float entradaMov = Input.GetAxisRaw("Horizontal");
        if(entradaMov == 0)//si no se pulsa tecla de movimiento decelera hasta parar el personaje
        {
            velActual = Mathf.MoveTowards(velActual,0,aceleracion*Time.deltaTime);
        }
        else{//si se activa, mueve el personaje
            velActual = Mathf.MoveTowards(velActual, entradaMov * velocidad, aceleracion * Time.deltaTime);
        }
        rb.velocity = new Vector2(Mathf.Clamp(velActual, -maxVelocidad, maxVelocidad), rb.velocity.y);
       
    }
}
