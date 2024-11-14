using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool hasWeapon = false;

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
}

