using UnityEngine;

public class MissionManager2 : MonoBehaviour
{    public static MissionManager2 Instance { get; private set; }

    private int totalPlants;
    private int wateredPlants = 0;

    public int TotalPlants => totalPlants;
    public int WateredPlants => wateredPlants;

    [SerializeField] GameObject landingZone;
    [SerializeField] Riego riegoScript;

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

        landingZone.SetActive(false);

        totalPlants = FindObjectsByType<OnCropWatering2>(FindObjectsSortMode.None).Length;

        if (totalPlants == 0)
        {
            Debug.LogError("No PlantColour scripts found in the scene! Check that your plants have the script attached.");
        }
        else
        {
            Debug.Log("Total Plants to water: " + totalPlants);
        }
    }

    void Start()
    {
    }

    public void PlantWatered()
    {
        wateredPlants++;
        if (MissionStatus2.Instance != null)
        {
            MissionStatus2.Instance.UpdateStatusText();
        }

        if (wateredPlants >= totalPlants)
        {
            PlayerWins();
        }
    }

    void PlayerWins()
    {
        Debug.Log("🎉 YOU WIN! All plants are watered! 🎉");
        landingZone.SetActive(true);

        if (riegoScript != null) 
        {
            riegoScript.particulasRiego.Stop();
            riegoScript.enabled = false;
        }
    }
}