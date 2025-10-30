using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingZone : MonoBehaviour
{
    bool stopSpam = false;
    bool isLandingZone = false;

    string userId;
    int levelNumber;
    string newStatus = "completado";
    string currentScene;

    [SerializeField] ProgressUpdateApi progressApi;
    [SerializeField] GameObject successPanel;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] LevelPointsManager levelPointsManager;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Level1":
                levelNumber = 1;
                break;
            case "Level2":
                levelNumber = 2;
                break;
            case "Level3":
                levelNumber = 3;
                break;
        }

        if (DataManager.instance != null)
        {
            userId = DataManager.instance.userId;
            DataManager.instance.currentLvl = levelNumber;
        }
        else
        {
            Debug.LogWarning("DataManager no encontrado.");
            userId = "TestUser";
        }
    }

    ///<summary>
    ///Si el usuario esta en la zona de aterrizaje y presiona "5" completa el desafio
    ///</summary>
    private void OnTriggerStay(Collider other)
    {
        if (stopSpam) { return; }

       if (isLandingZone && Input.GetKey(KeyCode.Alpha5))
        {

            GameOverManager.instance.ActivateGameOver();
            Debug.Log("Nivel Terminado");
            stopSpam = true;
            Debug.Log("SuccessPanel: " + successPanel);
            successPanel.SetActive(true);
            Debug.Log("SuccessPanel activado!");
            Debug.Log($"Enviando progreso: userId={userId}, nivel={levelNumber}, estado={newStatus}");
            progressApi.SendUpdate(userId, levelNumber, newStatus);
            Cursor.lockState = CursorLockMode.None;
            CalculateLevelPoints();
        }
    }
    ///<summary>
    ///Metodo que llama a <see cref="LevelPointsManager"/> para calcular los puntos en cada nivel
    ///</summary>
    private void CalculateLevelPoints()
    {
        switch (currentScene)
        {
            case "Level1":
                levelPointsManager.CalculatePointsLevel1();
                break;
            case "Level2":
                levelPointsManager.CalculatePointsLevel2();
                break;
            case "Level3":
                levelPointsManager.CalculatePointsLevel3();
                break;
        }
    }
    ///<summary>
    ///Detecta cuando el jugador toca la zona de aterrizaje
    ///</summary>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player"))
        {
            isLandingZone = true;
        }

        Debug.Log("isLanding: " + isLandingZone);
    }
    ///<summary>
    ///Detecta cuando el jugador sale de la zona de aterrizaje
    ///</summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLandingZone = false;
        }
        Debug.Log("isLanding: " + isLandingZone);
    }
    ///<summary>
    ///Metodo para ir a la escena de menu principal
    ///</summary>
    void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
