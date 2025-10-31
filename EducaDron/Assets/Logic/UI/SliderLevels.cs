using UnityEngine;
using UnityEngine.UI;
public class SliderLevels : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    AudioMixerManager audioManager;

    /// <summary>
    /// Carga los valores de volumenes ya guardados y asigna los
    /// listeners de los sliders para actualizar el volumen en tiempo
    /// real
    /// </summary>
    private void Start()
    {
        audioManager = Object.FindFirstObjectByType<AudioMixerManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioMixerManager no encontrado.");
            return;
        }

        float savedMasterLevel = PlayerPrefs.GetFloat("masterVolume", 0.75f);
        float savedSoundFXLevel = PlayerPrefs.GetFloat("soundFXVolume", 0.75f);
        float savedMusicLevel = PlayerPrefs.GetFloat("musicVolume", 0.75f);

        masterSlider.value = savedMasterLevel;
        sfxSlider.value = savedSoundFXLevel;
        musicSlider.value = savedMusicLevel;

        masterSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
        sfxSlider.onValueChanged.AddListener(audioManager.SetSoundFXVolume);
        musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);

    }
}
