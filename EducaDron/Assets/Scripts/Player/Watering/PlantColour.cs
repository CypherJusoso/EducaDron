using UnityEngine;

public class PlantColour : MonoBehaviour
{
    [SerializeField] private Color dryColor = Color.gray;

    [SerializeField] private Color wetColor = Color.yellow;

    private MeshRenderer[] allRenderers;
    private bool isWatered = false;

    void Start()
    {
        allRenderers = GetComponentsInChildren<MeshRenderer>();

     
        if (allRenderers.Length > 0)
        {
            foreach (var renderer in allRenderers)
            {
             
                renderer.material.color = dryColor;
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
                renderer.material.color = wetColor;
            }
        }

        GameManager.Instance.PlantWatered();
    }

    public bool IsWatered => isWatered;
}