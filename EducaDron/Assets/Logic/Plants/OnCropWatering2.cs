using UnityEngine;

public class OnCropWatering2 : MonoBehaviour, IWaterable
{
    [SerializeField] int particleHits = 0;
    [SerializeField] int plantHp = 20;

    [SerializeField] GameObject dryModel;
    [SerializeField] GameObject waterModel;

    public bool isWatered = false;
    public bool IsWatered => isWatered;

    private void Start()
    {
        dryModel.SetActive(true);
        waterModel.SetActive(false);
    }
    /// <summary>
    /// Procesa cuando el cultivo es regado y lo marca como completado al
    /// alcanzar el valor establecido
    /// </summary>
    public void ProcessWatering()
    {
        if (isWatered) { return; }

        particleHits++;
        if (particleHits >= plantHp)
        {
            isWatered = true;
            dryModel.SetActive(false);
            waterModel.SetActive(true);
            MissionManager2.Instance.PlantWatered();
        }
    }

}
