using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;

    // Referencia al DialogueManager para mostrar el diálogo
    public DialogueManager dialogueManager;

    private void Start()
    {
        // Si el DialogueManager no está asignado en el Inspector, intentamos obtenerlo automáticamente.
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Verifica que el jugador ha tocado la caja
        if (col.CompareTag("Player"))
        {
            if (GameManager.Instance != null && !GameManager.Instance.hasWeapon)
            {
                GameManager.Instance.hasWeapon = true;  // El jugador obtiene el arma

                // Crear el diálogo para mostrar al jugador que ha obtenido el arma
                Dialogue weaponDialogue = new Dialogue
                {
                    name = "Guía",
                    sentences = new string[]
                    {
                        "¡Estupendo! has encontrado una caja con balas, ya estas listo para enfrentar a cualquier enemigo que intente detenerte.",
                        "Además, tu arma incluye tecnología que permite cambiar entre dos tipos de disparo distintos, con diferentes alcances y potencias.",
                        "Prueba a disparar (K) y a cambiar el tipo de disparo (L). Tambien puedes apuntar hacia arriba (W). Sigamos explorando la cueva."
                       
                    }
                };

                // Inicia el diálogo
                dialogueManager.StartDialogue(weaponDialogue);

                StartCoroutine(DisplayMessage());
            }
        }
    }

    private IEnumerator DisplayMessage()
    {
        yield return new WaitForSeconds(3);  // Muestra el mensaje durante 3 segundos (puedes ajustarlo)
        // Aquí podrías ocultar el mensaje o hacer alguna acción adicional
    }
}
