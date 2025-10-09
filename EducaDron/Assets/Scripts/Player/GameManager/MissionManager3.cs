using TMPro;
using UnityEngine;

public class MissionManager3 : MonoBehaviour
{
    public static MissionManager3 instance;

    public int totalTargets = 3;
    public int fumigatedCrops = 0;

    [SerializeField] GameObject landingZone;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        landingZone.SetActive(false);
    }

    public void OnCropWatered()
    {
        fumigatedCrops++;

        if (fumigatedCrops >= totalTargets)
        {
            landingZone.SetActive(true);
        }
    }
}


