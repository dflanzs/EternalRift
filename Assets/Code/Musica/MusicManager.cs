using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public AudioClip[] sceneMusic; // Array de clips de música, asignados en el inspector


    void Awake()
    {
        
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex; // Obtén el índice de la escena actual

        if (sceneIndex < sceneMusic.Length && sceneMusic[sceneIndex] != null)
        {
            audioSource.clip = sceneMusic[sceneIndex];
            audioSource.Play();
        }
    }

    public void ChangeMusicWithFade(AudioClip newClip, float fadeDuration)
    {
        if (newClip != null){
        StartCoroutine(FadeOutIn(newClip, fadeDuration));
        }
        else
            StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutIn(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        // Fade Out
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade In
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / duration;
            yield return null;
        }
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Restablece el volumen original si se necesita reproducir otra pista
    }
}

