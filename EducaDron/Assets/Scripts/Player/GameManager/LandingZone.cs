using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingZone : MonoBehaviour
{
    bool stopSpam = false;
    bool isLandingZone = false;

    string userId;
    int levelNumber = 1;
    string newStatus = "completado";

    [SerializeField] ProgressUpdateApi progressApi;
    [SerializeField] GameObject successPanel;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] LevelPointsManager levelPointsManager;

    private void Start()
    {
         userId = DataManager.instance.userId;
        DataManager.instance.currentLvl = levelNumber;
    }
    private void OnTriggerStay(Collider other)
    {
        if (stopSpam) { return; }

       if (isLandingZone && Input.GetKey(KeyCode.Alpha5))
        {
            Debug.Log("Nivel Terminado");
            stopSpam = true;
            progressApi.SendUpdate(userId, levelNumber, newStatus);
            inputHandler.DisableInputs();
            successPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            levelPointsManager.CalculatePointsLevel1();
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
