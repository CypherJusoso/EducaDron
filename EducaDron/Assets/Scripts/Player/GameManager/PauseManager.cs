using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] InputHandler _input;
    public static bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (_input.menuOpenInput) 
        {
            if (isPaused) 
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            _input.menuOpenInput = false;
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();

        //TEST PARA EL EDITOR
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Debug.Log("Saliste del Videojuego!");
    }
}
