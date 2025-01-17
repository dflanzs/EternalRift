using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    // para medidas de accesibilidad


    //daño visual
    public Color damageColor = Color.red; 
    private SpriteRenderer spriteRenderer;
    private Color color_original;
    public float damageEffectDuration = 0.5f;  // Duración del parpadeo


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
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color_original = spriteRenderer.color;  // Guarda el color original
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        StartCoroutine(FlashDamageEffect());
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
        
        MutationBar mutationBar = FindObjectOfType<MutationBar>();
    if (mutationBar != null)
    {
        mutationBar.ResetCharge();
    }
        SceneManager.LoadScene("copia_escena_1");
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
        if(GameManager.Instance == null)
            Debug.LogWarning("No has puesto el GameManager");

        if (GameManager.Instance != null && GameManager.Instance.fallDamage)
            return; // No aplicar daño si la opción de recibir daño está desactivada


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
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        // Daño por contacto con enemigo
        if (collision.gameObject.CompareTag("npc"))
        {
            TakeDamage(20); // Ajustar el daño según sea necesario
        } else if(collision.gameObject.CompareTag("enemyBullets")) {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            TakeDamage((int) bullet.Damage);
        }
    }

    // Daño por contacto con lava en escena principal.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.fallDamage)
            return; // No aplicar daño si la opción de recibir daño está desactivada
            
        if (other.CompareTag("malo") || other.CompareTag("puerta"))
        {
            TakeDamage(10); // Ajustar el daño según sea necesario
        }

        
    }

    private IEnumerator FlashDamageEffect()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageEffectDuration);
        spriteRenderer.color = color_original;
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
