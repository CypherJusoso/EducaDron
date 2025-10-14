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


    private void OnTriggerStay(Collider other)
    {
        if (stopSpam) { return; }

       if (isLandingZone && Input.GetKey(KeyCode.Alpha5))
        {
            Debug.Log("Nivel Terminado");
            stopSpam = true;
            Debug.Log("SuccessPanel: " + successPanel);
            successPanel.SetActive(true);
            Debug.Log("SuccessPanel activado!");
            Debug.Log($"Enviando progreso: userId={userId}, nivel={levelNumber}, estado={newStatus}");
            progressApi.SendUpdate(userId, levelNumber, newStatus);
            inputHandler.DisableInputs();
            Cursor.lockState = CursorLockMode.None;
            CalculateLevelPoints();
        }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player"))
        {
            isLandingZone = true;
        }

        Debug.Log("isLanding: " + isLandingZone);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLandingZone = false;
        }
        Debug.Log("isLanding: " + isLandingZone);
    }

    void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
