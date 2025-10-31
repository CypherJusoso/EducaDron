using TMPro;
using UnityEngine;

public class Coordinates : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coordinates;
    [SerializeField] GameObject playeDrone;

    /// <summary>
    /// Actualiza el texto en pantalla con las coordenadas actuales del dron
    /// </summary>
    void Update()
    {
        if (playeDrone != null && coordinates != null) 
        {
            coordinates.text = $"X: {playeDrone.transform.position.x:F2}\nY: {playeDrone.transform.position.y:F2}\nZ: {playeDrone.transform.position.z:F2}";
        }
    }
}
