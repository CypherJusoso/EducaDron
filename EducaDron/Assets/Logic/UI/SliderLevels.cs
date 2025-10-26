using UnityEngine;
using UnityEngine.UI;
public class SliderLevels : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    AudioMixerManager audioManager;

    private void Start()
    {
        audioManager = Object.FindFirstObjectByType<AudioMixerManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioMixerManager not found! Make sure it exists in the Bootstrap scene.");
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
