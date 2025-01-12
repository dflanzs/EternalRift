using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundEffects : MonoBehaviour
{
  public AudioSource audioSource;  // Fuente de audio.
    public AudioClip[] footstepSounds;  // Clips de pasos.
    public AudioClip jumpSounds;  // Clips de saltos.
    public AudioClip dashSounds;  // Clips de dash.

    public AudioClip keySound;
    public Player player;

    
    private void Start(){
        GameObject sonido = GameObject.Find("PlayerSoundManager");
        if (sonido != null)
        audioSource = sonido.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && player != null && player.CheckIfGrounded()){
            PlayJumpSound();
        }
    }
    public void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            // Seleccionar un sonido aleatorio del array.
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[index]);
        }
    }


    public void PlayJumpSound()
    {
        StartCoroutine(PlayJumpSoundCoroutine());
    }

    private IEnumerator PlayJumpSoundCoroutine()
    {
        // Aumentar el volumen para el salto.
        float startVolume = audioSource.volume;
        audioSource.volume = audioSource.volume * 1.5f;
        audioSource.PlayOneShot(jumpSounds);

        // Esperar a que el sonido termine.
        yield return new WaitForSeconds(jumpSounds.length);

        // Restaurar el volumen al nivel normal.
        audioSource.volume = startVolume;
    }

    public void PlayDashSound()
    {
            audioSource.PlayOneShot(dashSounds);
    }

    public void PlayKeySound()
    {
            audioSource.PlayOneShot(keySound);
    }
}
