using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleClip : MonoBehaviour
{
    public AudioSource audioSource;       // Asigna el AudioSource desde el Inspector.
    public AudioClip[] audioClips;       // Arrastra los clips aquí desde el Inspector.
    private static MultipleClip instance;

    void Awake()
    {
        
            instance = this;
            DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (audioClips.Length > 0 && audioSource != null)
        {
            PlayClip(0); // Reproduce el primer clip como ejemplo.
        }
    }

    public void PlayClip(int index)
    {
        if (index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango.");
        }
    }
}

