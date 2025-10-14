using UnityEngine;

public class OnCropWatering : MonoBehaviour
{
    [SerializeField] int particleHits = 0;
    [SerializeField] int plantHp = 20;
    [SerializeField] GameObject flyParticle;
    [SerializeField] GameObject purpleAura;

    public bool isWatered = false;
    
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision with: " + other.name);

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
            MissionManager3.instance.OnCropWatered();
            if (flyParticle != null)
                flyParticle.SetActive(false);

            if (purpleAura != null)
                purpleAura.SetActive(false);

            Debug.Log("Plant fully fumigated!");
        }
    }
}
