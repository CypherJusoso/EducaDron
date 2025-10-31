using UnityEngine;
using UnityEngine.Audio;
public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Ajusta el audio para el volumen maestro y guarda la configuracion
    /// </summary>
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("masterVolume", level);
    }
    /// <summary>
    /// Ajusta el audio para los efectos de sonido y guarda la configuracion
    /// </summary>
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("soundFXVolume", level);
    }

    /// <summary>
    /// Ajusta el audio para la musica y guarda la configuracion
    /// </summary>
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("musicVolume", level);
    }
}
