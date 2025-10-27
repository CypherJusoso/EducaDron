using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource soundFXObject;
    [SerializeField] AudioSource musicObject;
    [SerializeField] AudioSource loopingSFXSource;
    [SerializeField] AudioClip mainMenuTheme;
    [SerializeField] AudioClip level1Theme;
    [SerializeField] AudioClip level2Theme;
    [SerializeField] AudioClip level3Theme;
    [SerializeField] AudioMixerGroup musicMixerGroup;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("AudioManager Created in Scene: " + SceneManager.GetActiveScene().name);

        }
        else
        {
            Debug.Log("Duplicate AudioManager found in Scene: " + SceneManager.GetActiveScene().name + " — destroying it!");

            SceneManager.sceneLoaded -= instance.OnSceneLoaded;
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void PlaySoundFXClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity, transform);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    
    }

    public void PlayRandomSoundFXClip(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, clips.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity, transform);

        audioSource.clip = clips[rand];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayLoopingSFX(AudioClip clip, Transform followTransform, float volume)
    {
        if (loopingSFXSource == null)
        {
            loopingSFXSource = Instantiate(soundFXObject, followTransform.position, Quaternion.identity, transform);
            loopingSFXSource.loop = true;
            loopingSFXSource.volume = volume;
            loopingSFXSource.clip = clip;
            loopingSFXSource.Play();
        }
    }

    public void StopLoopingSFX()
    {
        if (loopingSFXSource != null)
        {
            loopingSFXSource.Stop();
            Destroy(loopingSFXSource.gameObject);
            loopingSFXSource = null;
        }
    }

    public void PlayMusic(AudioClip musicClip, bool loop)
    {
        if (musicObject == null)
        {
            musicObject = gameObject.AddComponent<AudioSource>();
            musicObject.playOnAwake = false;
            musicObject.outputAudioMixerGroup = musicMixerGroup;
        }

        if (musicObject.isPlaying && musicObject.clip == musicClip) { return; }

        musicObject.clip = musicClip;
        musicObject.loop = loop;
        musicObject.Play();
    }

    public void StopMusic()
    {
        if (musicObject != null) 
        {
            Debug.Log("Stopping music: " + musicObject.clip?.name);

            musicObject.Stop(); 
            musicObject.clip = null;
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "MainMenu" ||
            scene.name == "ChooseLevel" ||
            scene.name == "Ranking" ||
            scene.name == "LoginScene" ||
            scene.name == "RegisterScene")
        {
           Debug.Log("Playing Menu Music!");

            PlayMusic(mainMenuTheme, true);
        }
        else if (scene.name == "Level1")
        {
            StopMusic();
         //   PlayMusic(level1Theme, true);
        }
        else if (scene.name == "Level2")
        {
            StopMusic();

            //    PlayMusic(level2Theme, true);
        }
        else if (scene.name == "Level3")
        {
            StopMusic();

            //    PlayMusic(level3Theme, true);
        }
    }
}
