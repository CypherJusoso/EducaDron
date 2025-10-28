using UnityEngine;

public class OnCropWatering : MonoBehaviour, IWaterable
{
    [SerializeField] int particleHits = 0;
    [SerializeField] int plantHp = 20;

    [SerializeField] GameObject flyParticle;
    [SerializeField] GameObject purpleAura;

    public bool isWatered = false;
    public bool IsWatered => isWatered;

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
            MissionManager3.instance.OnCropWatered();
            if (flyParticle != null)
                flyParticle.SetActive(false);

            if (purpleAura != null)
                purpleAura.SetActive(false);

            Debug.Log("Plant fully fumigated!");
        }
    }

}
