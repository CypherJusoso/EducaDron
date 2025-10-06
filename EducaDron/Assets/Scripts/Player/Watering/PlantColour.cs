using UnityEngine;

public class PlantColour : MonoBehaviour
{
    [SerializeField] private Color dryColor = Color.gray;

    private Color originalWetColor;

    private MeshRenderer[] allRenderers;
    private bool isWatered = false;

    private const string BASE_COLOR_PROPERTY = "_BaseColor";

    void Start()
    {
        allRenderers = GetComponentsInChildren<MeshRenderer>();

        if (allRenderers.Length > 0)
        {
 
            if (allRenderers[0].material.HasProperty(BASE_COLOR_PROPERTY))
            {
                originalWetColor = allRenderers[0].material.GetColor(BASE_COLOR_PROPERTY);
            }
            else 
            {
                originalWetColor = allRenderers[0].material.color;
            }

            foreach (var renderer in allRenderers)
            {
                renderer.material.SetColor(BASE_COLOR_PROPERTY, dryColor);
                renderer.material.SetColor("_Color", dryColor); 
            }
        }
        else
        {
            Debug.LogWarning("PlantColour script on " + gameObject.name + " could not find any MeshRenderers in children. Check your LOD setup.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && !isWatered)
        {
            WaterPlant();
        }
    }

    void WaterPlant()
    {
        isWatered = true;

        if (allRenderers.Length > 0)
        {
            foreach (var renderer in allRenderers)
            {
                renderer.material.SetColor(BASE_COLOR_PROPERTY, originalWetColor);
                renderer.material.SetColor("_Color", originalWetColor);
            }
        }

        GameManager.Instance.PlantWatered();
    }

    public bool IsWatered => isWatered;
}