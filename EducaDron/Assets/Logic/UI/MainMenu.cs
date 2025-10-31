using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button SelectButton;
    [SerializeField] Button ScoreboardButton;
    [SerializeField] Button RegisterButton;
    [SerializeField] Button LoginButton;
    [SerializeField] LogoutApi logoutApi;

    private void OnEnable()
    {
        RefreshButtons();
    }

    /// <summary>
    /// Inicializa los botones según el estado de sesión.
    /// </summary>
    private void Start()
    {
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        bool loggedIn = !string.IsNullOrEmpty(DataManager.instance?.userId);

        if (SelectButton != null)
            SelectButton.interactable = loggedIn;

        if (ScoreboardButton != null)
            ScoreboardButton.interactable = loggedIn;

        if (RegisterButton != null)
        {
            RegisterButton.interactable =!loggedIn;
        }

        if (LoginButton != null)
        {
            LoginButton.interactable = !loggedIn;
        }

    }

    /// <summary>
    /// Carga la escena de registro de usuario
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Carga la escena de seleccionar nivel
    /// </summary>
    public void SelectLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    /// <summary>
    /// Botón para salir (logout + cerrar app). Deshabilita botones de inmediato.
    /// </summary>
    public void Quit()
    {
        if (SelectButton != null) SelectButton.interactable = false;
        if (ScoreboardButton != null) ScoreboardButton.interactable = false;
        if (RegisterButton != null) RegisterButton.interactable = true;
        if(LoginButton != null) LoginButton.interactable = true;
        StartCoroutine(QuitFlow());
    }

    private IEnumerator QuitFlow()
    {
        if (logoutApi != null)
            yield return logoutApi.Logout();

        var dm = DataManager.instance;
        if (dm != null)
        {
            dm.userId = null;
            dm.username = null;
            dm.email = null;
            dm.currentLvl = 0;
            dm.ResetPoints();
        }

        Debug.Log("Player Has Quit The Game");

        // Cerrar la aplicación solo en builds de escritorio (Windows). En editor, detener Play.
        ExitApplication();
        yield break;
    }

    /// <summary>
    /// Cierra la aplicación según la plataforma.
    /// </summary>
    private void ExitApplication()
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
#elif UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // En WebGL u otras plataformas, no se hace nada (no se puede cerrar la ventana desde runtime).
    }

    /// <summary>
    /// Carga la escena para iniciar sesion
    /// </summary>
    public void Login()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Quiz()
    {
        SceneManager.LoadScene("Quiz");
    }

    public void RedirectMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Carga la escena para ir a la tabla de puntajes
    /// </summary>
    public void Scoreboard()
    {
        SceneManager.LoadScene("Ranking");
    }
}
