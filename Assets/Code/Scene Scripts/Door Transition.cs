using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Puerta"))
        {
            // Carga la siguiente escena en el índice de Build Settings
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Verifica que el índice no exceda el número de escenas en el proyecto
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            
        }
    }
}

