using UnityEngine;

public class AccessibilityOptions : MonoBehaviour
{
    public bool fallDamageEnabled = true;
    public bool autoShootEnabled = true;

    // Método que se conecta al botón en el panel de UI
    public void ToggleFallDamage()
    {
        fallDamageEnabled = !fallDamageEnabled;
        Debug.Log($"Daño por caídas {(fallDamageEnabled ? "activado" : "desactivado")}");
    }

    // Método para cambiar el estado del disparo automático desde la UI
    public void ToggleAutoShoot()
    {
        autoShootEnabled = !autoShootEnabled;
        Debug.Log($"Disparo automático {(autoShootEnabled ? "activado" : "desactivado")}");
    }
}

