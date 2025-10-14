using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantScanner : MonoBehaviour
{
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] LayerMask plantLayerMask;

    [SerializeField] GameObject alertUI;
    [SerializeField] TextMeshProUGUI alertText;

    Collider[] closePlants;
    PlantColour plantColour;
    bool isInRange = false;
    void Update()
    {
        DetectNearbyPlants();
    }

    void DetectNearbyPlants()
    {
        closePlants = Physics.OverlapSphere(transform.position, detectionRadius, plantLayerMask);

        if (closePlants.Length > 0)
        {
            Collider closest = GetClosestPlant(closePlants);

            if (closest != null)
            {
                OnCropWatering2 crop = closest.GetComponent<OnCropWatering2>();

                if (crop != null && !crop.isWatered) 
                {
                    if (!isInRange)
                    {
                        isInRange = true;
                        ShowIndicator(true);
                    }
                }
                else
                {
                    if (isInRange)
                    {
                        isInRange = false;
                        ShowIndicator(false);
                    }
                }
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                ShowIndicator(false);
            }
        }
        
    }
    Collider GetClosestPlant(Collider[] plants)
    {
        Collider closest = null;
        float minDistnace = Mathf.Infinity;

        foreach (Collider collider in plants)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistnace)
            {
                minDistnace = distance;
                closest = collider;
            }
        }
        return closest;
    }

    void ShowIndicator(bool show)
    {
        if (alertUI != null)
        {
            alertUI.SetActive(show);
        }

        if (alertText != null)
        {
            alertText.text = show ? "Cultivo detectado -Presiona R para regar" : "";
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
