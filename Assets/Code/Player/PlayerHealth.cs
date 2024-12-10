using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;

    public Slider healthBar;

    // Variables empleadas en el daño por caida
    [SerializeField] private float fallThreshold = 15f; // Altura mínima para recibir daño
    [SerializeField] private int fallDamagePerUnit = 1; // Daño por unidad de altura que supere el umbral
    private float startFallHeight;
    private bool isFalling;

    // GroundCheck variables
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleFallDamage();
    }


    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log($"Daño recibido: {damage}. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Lógica para manejar la muerte
    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Vuelve a empezar la escena inicial
    }

    // Actualizar la barra de vida
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    // Método para verificar si el jugador está tocando el suelo
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleFallDamage()
    {
        if (!IsGrounded() && !isFalling)
        {
            // El jugador empieza a caer
            isFalling = true;
            startFallHeight = transform.position.y;
            Debug.Log($"Altura inicial de caída: {startFallHeight}");
        }
        else if (IsGrounded() && isFalling)
        {
            // El jugador aterriza
            isFalling = false;
            float fallDistance = startFallHeight - transform.position.y;
            Debug.Log($"Distancia de caída: {fallDistance}");

            if (fallDistance > fallThreshold)
            {
                int damage = Mathf.CeilToInt((fallDistance - fallThreshold) * fallDamagePerUnit);
                TakeDamage(damage);
                Debug.Log($"Daño por caída: {damage}. Distancia de caída: {fallDistance}");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Daño por agua (escena tutorial)
        if (collision.gameObject.CompareTag("Agua") || collision.gameObject.CompareTag("malo"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Daño por contacto con enemigo
        /*if (collision.gameObject.CompareTag("npc") || collision.gameObject.CompareTag("enemyBullets"))
        {
            TakeDamage(20); // Ajustar el daño según sea necesario
        }*/
    }

    // Daño por contacto con lava en escena principal.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("malo"))
        {
            TakeDamage(10); // Ajustar el daño según sea necesario
        }
    }

    // Método para recuperar vida (opcional)
    /* 
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Evitar que supere el máximo
        Debug.Log($"Vida restaurada: {amount}. Vida actual: {currentHealth}");
    }
    */
}
