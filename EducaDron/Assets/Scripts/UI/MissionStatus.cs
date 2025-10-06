using TMPro;
using UnityEngine;

public class MissionStatus : MonoBehaviour
{
    [SerializeField] GameObject missionPanel;
    [SerializeField] TextMeshProUGUI missionText;
    [SerializeField] TextMeshProUGUI remainingPhotosText;
    [SerializeField] PhotoCapture photoCapture;

    int remainingPhotos;
    void Update()
    {
        remainingPhotos = 10 - photoCapture.actualPhotos;
        if (remainingPhotos < 0) 
        {
            remainingPhotos = 0;
        }
        PressTab();
    }

    private void PressTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UpdateMissionText();
            missionPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            missionPanel.SetActive(false);
        }
    }

    void UpdateMissionText()
    {
        int current = MissionManager.instance.photosTaken;
        int total = MissionManager.instance.totalTargets;

        missionText.text = "Mision actual:\n- Fotografiar cultivos: " + current + " / " + total;
        remainingPhotosText.text =  "Fotos restantes:" + remainingPhotos;
    }
}
