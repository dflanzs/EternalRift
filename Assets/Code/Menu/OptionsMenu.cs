using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    [SerializeField] private Toggle fallDamage;
    [SerializeField] private Toggle autoShoot;
    public Slider volumeMusicaSlider, volumeSonidoSlider;
    public AudioSource audioSourceMusica;
    public AudioSource[] audioSourceSonido;



    private void Start(){
        GameObject musica_ = GameObject.Find("MusicManager");
        if (musica_ != null)
            audioSourceMusica = musica_.GetComponent<AudioSource>();

        GameObject sonidoAmbiente = GameObject.Find("AmbientManager");
        if (sonidoAmbiente != null)
            audioSourceSonido[1] = sonidoAmbiente.GetComponent<AudioSource>();

        GameObject sonidojugador = GameObject.Find("PlayerSoundManager");
        if (sonidojugador != null)
            audioSourceSonido[2] = sonidojugador.GetComponent<AudioSource>();

        fallDamage.isOn = GameManager.Instance.fallDamage; 
        autoShoot.isOn = GameManager.Instance.autoShoot;
        if (volumeMusicaSlider != null && audioSourceMusica != null)
        {
            // Configura el Slider para que ajuste el volumen inicial
            volumeMusicaSlider.value = audioSourceMusica.volume;

            // Suscríbete al evento del Slider
            volumeMusicaSlider.onValueChanged.AddListener(SetVolumeMusica);
        }
        else
        {
            Debug.LogError("Faltan referencias en VolumeController.");
        }

        if (volumeSonidoSlider != null && audioSourceSonido != null)
        {
            // Configura el Slider para que ajuste el volumen inicial
            for(int i = 0; i < audioSourceSonido.Length; i++)
            {
                volumeSonidoSlider.value = audioSourceSonido[i].volume;
            }

            // Suscríbete al evento del Slider
            volumeSonidoSlider.onValueChanged.AddListener(SetVolumeSonido);
        }
        else
        {
            Debug.LogError("Faltan referencias en VolumeController.");
        }
    
    }

    public void ToggleAutoShoot(bool value){
        GameManager.Instance.autoShoot = !GameManager.Instance.autoShoot;
    }

    public void ToggleFallDamage(bool value){
        GameManager.Instance.fallDamage = !GameManager.Instance.fallDamage;
    }

    public void SetVolumeMusica(float volume)
    {
        if (audioSourceMusica != null)
        {
            audioSourceMusica.volume = volume; // Ajusta el volumen del AudioSource
        }
    }

    public void SetVolumeSonido(float volume)
    {
        for(int i = 0; i < audioSourceSonido.Length; i++)
        {
            if (audioSourceSonido[i] != null)
        {
            audioSourceSonido[i].volume = volume; // Ajusta el volumen del AudioSource
        }
        }
    }

}
