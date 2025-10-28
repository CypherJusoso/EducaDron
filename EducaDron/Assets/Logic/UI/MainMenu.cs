using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Button SelectButton;

    /// <summary>
    /// Inicializa el boton para seleccionar nivel como disabled,
    /// si se encuentra un userId el boton se activa
    /// </summary>
    private void Start()
    {
        if (SelectButton != null)
        {
           SelectButton.interactable = false;

        if (!string.IsNullOrEmpty(DataManager.instance.userId))
        {
            SelectButton.interactable = true;
        }
        }
    }
    /// <summary>
    /// Carga la escena de registro de usuario
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /*
    public void MainMenuDirect()
    {
        SceneManager.LoadScene("MainMenu");
    }
*/
    /// <summary>
    /// Carga la escena de seleccionar nivel
    /// </summary>
    public void SelectLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    /// <summary>
    /// Boton para salir del simulador
    /// </summary>
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
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
    /// <summary>
    /// Carga la escena para ir a la tabla de puntajes
    /// </summary>
    public void Scoreboard()
    {
        SceneManager.LoadScene("Ranking");

    }
}
