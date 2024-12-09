using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public Slider healthBar;

  

    // Método para recibir daño
    private void TakeDamage(int damage)
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
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Actualizar la barra de vida
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float) currentHealth / maxHealth;
        }
    }

    // Detectar daño por colisiones
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        // Daño por entorno dañino (como lava o agua)
        if (collision.gameObject.CompareTag("Agua") || collision.gameObject.CompareTag("malo"))
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //TakeDamage(10); // Ajusta el daño según sea necesario
            
        }

        // Daño por contacto con enemigo
        if (collision.gameObject.CompareTag("npc") || collision.gameObject.CompareTag("enemyBullets") )
        {
            TakeDamage(20); // Ajusta el daño según sea necesario
            
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)  {
        if (other.CompareTag("malo")){
            TakeDamage(10); // Ajusta el daño según sea necesario
        }

  
}
}





