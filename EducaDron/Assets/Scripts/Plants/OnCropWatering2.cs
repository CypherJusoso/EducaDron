using UnityEngine;

public class OnCropWatering2 : MonoBehaviour
{
    [SerializeField] int particleHits = 0;
    [SerializeField] int plantHp = 20;

    [SerializeField] GameObject dryModel;
    [SerializeField] GameObject waterModel;

    public bool isWatered = false;

    private void Start()
    {
        dryModel.SetActive(true);
        waterModel.SetActive(false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("WateringSystem"))
        {
            ProcessWatering();
        }
    }

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

    public bool IsWatered => isWatered;
}
