using TMPro;
using UnityEngine;

public class MissionStatus3 : MonoBehaviour
{
    [SerializeField] GameObject missionPanel;
    [SerializeField] TextMeshProUGUI missionText;


    // Update is called once per frame
    void Update()
    {
        ShowMissionStatus();
    }

    private void ShowMissionStatus()
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
        int current = MissionManager3.instance.fumigatedCrops;
        int total = MissionManager3.instance.totalTargets;

        missionText.text = "Mision actual:\n- Fumigar cultivos: " + current + " / " + total;
    }
}
