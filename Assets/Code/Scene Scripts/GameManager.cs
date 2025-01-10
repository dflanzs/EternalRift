using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool hasWeapon = false;
    public bool autoShoot = false;
    public bool fallDamage = false;
    public DialogueManager dialogueManager;

    private void Awake()
    {
        // Si ya existe una instancia, destruir el duplicado
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Mantener el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject);  // Evita duplicados en nuevas escenas
        }
    }

    private void Start()
    {
        // Asegúrate de que solo el DialogueManager en las escenas de tutorial y principal persista
        SceneManager.sceneLoaded += OnSceneLoaded; // Llamar cuando se carga una nueva escena
    }

    // Se llama cuando se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuPrincipal")
        {
            // Destruir el DialogueManager y Canvas si estamos en la pantalla de inicio
            Destroy(GameObject.Find("DialogueManager"));
        }
        else
        {
            // Verificar si el DialogueManager no existe, entonces crearlo
            if (dialogueManager == null)
            {
                dialogueManager = FindObjectOfType<DialogueManager>();
            }

            // Evitar duplicados del DialogueManager
            if (dialogueManager == null)
            {
                GameObject dialogueManagerPrefab = Resources.Load<GameObject>("DialogueManagerPrefab");
                Instantiate(dialogueManagerPrefab);
            }
        }
    }

    // Limpiar el delegado cuando ya no sea necesario
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
