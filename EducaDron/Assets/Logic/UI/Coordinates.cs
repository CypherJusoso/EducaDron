using TMPro;
using UnityEngine;

public class Coordinates : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coordinates;
    [SerializeField] GameObject playeDrone;

    // Update is called once per frame
    void Update()
    {
        if (playeDrone != null && coordinates != null) 
        {
            coordinates.text = $"X: {playeDrone.transform.position.x:F2}\nY: {playeDrone.transform.position.y:F2}\nZ: {playeDrone.transform.position.z:F2}";
        }
    }
}
