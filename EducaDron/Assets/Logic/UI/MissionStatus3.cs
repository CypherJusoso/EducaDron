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

    /// <summary>
    /// Muestra y oculta la interfaz que muestra
    /// el estado de la mision 
    /// </summary>
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
    /// <summary>
    /// Actualiza el texto del estado de mision con las plantas
    /// fumigadas y las que faltan por fumigar
    /// </summary>
    void UpdateMissionText()
    {
        int current = MissionManager3.instance.fumigatedCrops;
        int total = MissionManager3.instance.totalTargets;

        missionText.text = "Mision actual:\n- Fumigar cultivos: " + current + " / " + total;
    }
}
