using TMPro;
using UnityEngine;

public class MissionStatus : MonoBehaviour
{
    [SerializeField] GameObject missionPanel;
    [SerializeField] TextMeshProUGUI missionText;

    // Update is called once per frame
    void Update()
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
    }
}
