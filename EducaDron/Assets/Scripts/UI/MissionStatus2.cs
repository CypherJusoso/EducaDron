using TMPro;
using UnityEngine;

public class MissionStatus2 : MonoBehaviour
{
    public static MissionStatus2 Instance { get; private set; }

    [SerializeField] private GameObject statusPanel;
    [SerializeField] private TextMeshProUGUI missionGoalText;
    [SerializeField] private TextMeshProUGUI remainingPlantsText;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    private GameManager gameManager;

    void Awake() 
    {
       
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

      
        gameManager = GameManager.Instance;

  
        if (statusPanel != null)
        {
            statusPanel.SetActive(false);
        }


        if (gameManager != null)
        {
            UpdateStatusText();
        }
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        
        if (Input.GetKeyDown(toggleKey))
        {
      
            UpdateStatusText();
            statusPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(toggleKey))
        {
            statusPanel.SetActive(false);
        }
    }
    public void UpdateStatusText()
    {
        if (gameManager == null)
        {
            Debug.LogError("MissionStatus2 cannot find the GameManager instance!");
            return;
        }

        int currentWatered = gameManager.WateredPlants;
        int totalPlants = gameManager.TotalPlants;

        int remaining = totalPlants - currentWatered;

        missionGoalText.text = "Misión Actual:\n- Regar todas las plantas: " + currentWatered + " / " + totalPlants;
        remainingPlantsText.text = "Plantas restantes por regar: " + remaining;
    }
}