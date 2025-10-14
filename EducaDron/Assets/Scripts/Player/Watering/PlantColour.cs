using UnityEngine;

public class PlantColour : MonoBehaviour
{

    [SerializeField] GameObject dryModel;
    [SerializeField] GameObject wateredModel;

    bool isWatered = false;

    void Start()
    {
        dryModel.SetActive(true);
        wateredModel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WateringSystem") && !isWatered)
        {
            WaterPlant();
        }
    }

    public void HighlightPlant(bool highlight)
    {
        Renderer[] renderers = dryModel.GetComponentsInChildren<Renderer>();
        Color color = highlight ? Color.cyan * 0.5f : Color.black;

        foreach (var r in renderers)
        {
            r.material.SetColor("_EmissionColor", color);
        }
    }

    void WaterPlant()
    {
        if (isWatered) { return; }

        isWatered = true;

        dryModel.SetActive(false);
        wateredModel.SetActive(true);
        Debug.Log($" {name} regado correctamente!");

        MissionManager2.Instance.PlantWatered();
    }

    public bool IsWatered => isWatered;
}