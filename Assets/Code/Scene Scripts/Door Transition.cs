using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTransition : MonoBehaviour
{
    public DialogueManager dialogueManager;  // Referencia al DialogueManager
    public string[] noWeaponDialogue;  // Diálogo que se muestra si el jugador no tiene el arma

    private void Start()
    {
        // Asegúrate de que el DialogueManager esté asignado
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Puerta"))
        {
            // Verifica si el jugador tiene el arma
            if (!GameManager.Instance.hasWeapon)
            {
                // Si no tiene el arma, muestra un diálogo y no permite la transición
                Dialogue warningDialogue = new Dialogue
                {
                    name = "Guía",
                    sentences = noWeaponDialogue  // Usa el diálogo que se pasa en el inspector
                };

                dialogueManager.StartDialogue(warningDialogue);  // Muestra el diálogo
                return;  // Detiene la ejecución y no permite la transición
            }

            // Si el jugador tiene el arma, permite la transición a la siguiente escena
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Verifica que el índice no exceda el número de escenas en el proyecto
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}
