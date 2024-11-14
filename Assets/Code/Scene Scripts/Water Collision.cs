using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterCollisiion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Agua"))
        {
            // Reinicia el nivel actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
