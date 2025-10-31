using UnityEngine;

public class MissionManager2 : MonoBehaviour
{    public static MissionManager2 Instance { get; private set; }

    private int totalPlants;
    private int wateredPlants = 0;

    public int TotalPlants => totalPlants;
    public int WateredPlants => wateredPlants;

    [SerializeField] GameObject landingZone;

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
    }

    ///<summary>
    ///Detecta cuando un cultivo es regado y actualiza el estado de la mision
    ///</summary>
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
    ///<summary>
    ///Este metodo activa la zona de aterrizaje cuando el jugador completa el desafio
    ///</summary>
    void PlayerWins()
    {
        landingZone.SetActive(true);
    }
}