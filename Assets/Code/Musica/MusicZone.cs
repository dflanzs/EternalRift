using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioClip newMusic; // Clip de música que se reproducirá en esta zona
    public float fadeTime;
    private MusicManager musicManager; // Referencia al MusicManager

    void Start()
    {
        // Encuentra el MusicManager en la escena
        musicManager = FindObjectOfType<MusicManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            if (musicManager != null)
            {
                musicManager.ChangeMusicWithFade(newMusic, fadeTime);
            }
        }
    }
}
