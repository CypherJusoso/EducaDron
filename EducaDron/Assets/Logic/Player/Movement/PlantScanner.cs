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
    bool isInRange = false;
    void Update()
    {
        DetectNearbyPlants();
    }

    /// <summary>
    /// Detecta cuando hay cultivos cercanos que necesitan riego, mostrando una alerta para avisar al usuario
    /// </summary>
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
    /// <summary>
    /// Determina cual de los cultivos detectados esta mas cerca del dron
    /// </summary>
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

    /// <summary>
    /// Activa y desactiva la alerta de que hay un cultivo que necesita ser regado
    /// </summary>
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
    /// <summary>
    /// Dibuja en el editor la esfera de rango del metodo <see cref="DetectNearbyPlants"/>
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
