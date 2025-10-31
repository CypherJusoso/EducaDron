using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] AudioClip clickButtonSound;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }
    /// <summary>
    /// Llama a <see cref="AudioManager"/> para reproducir un sonido cuando se presiona un boton
    /// </summary>
    void PlayClickSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySoundFXClip(clickButtonSound, transform, 1f);
        }
    }
}
