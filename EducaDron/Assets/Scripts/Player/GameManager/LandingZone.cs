using UnityEngine;

public class LandingZone : MonoBehaviour
{
    bool stopSpam = false;
    bool isLandingZone = false;
    private void OnTriggerStay(Collider other)
    {
        if (stopSpam) { return; }

       if (isLandingZone && Input.GetKey(KeyCode.Alpha5))
        {
            Debug.Log("Nivel Terminado");
            stopSpam = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player"))
        {
            isLandingZone = true;
        }

        Debug.Log("isLanding: " + isLandingZone);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLandingZone = false;
        }
        Debug.Log("isLanding: " + isLandingZone);
    }
}
