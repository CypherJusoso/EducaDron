using Microlight.MicroBar;
using UnityEngine;

public class Watering : MonoBehaviour
{
    [Header("Tanque de Agua")]
    public const float WATER_CAPACITY = 1000f;
    public float aguaActual = 1000f;
    public float velocidadRiego = 10f; // Litros por segundo
    private AudioSource currentWaterSound;


    [Header("Riego")]
    [SerializeField] float wateringRadius = 3f;
    [SerializeField] LayerMask plantLayerMask;
    [SerializeField] int waterPerSecond = 5;
    [SerializeField] ParticleSystem waterParticles;
    [SerializeField] AudioClip waterClip;
    [SerializeField] Transform soundOrigin;

    [Header("UI")]
    [SerializeField] MicroBar waterMicroBar;
    [SerializeField] GameObject failurePanel;


    [Header("Player")]
    [SerializeField] PlayerMover3 playerMover;


    bool isWatering = false;
    private void Start()
    {
        StopWatering();
        if (waterMicroBar != null) waterMicroBar.Initialize(WATER_CAPACITY);
    }
    void Update()
    {
        if (Dialogue.isDialoguePlaying) { return; }
        if (PauseManager.isPaused) { return; }

        if (Input.GetKey(KeyCode.R))
        {
            if (!isWatering) StartWatering();
        }
        else
        {
            if (isWatering) StopWatering();
        }

        if (isWatering)
        {
            WaterPlantsInRange();
            aguaActual -= velocidadRiego * Time.deltaTime;
            aguaActual = Mathf.Max(aguaActual, 0f);

            if (aguaActual <= 0 && isWatering)
            {
                StopWatering();
                GameOver();
            }
        }

        if (waterMicroBar != null)
        {
            waterMicroBar.UpdateBar(aguaActual, false, UpdateAnim.Damage);
        }      
    }

    void StartWatering()
    {
        isWatering = true;
        if (waterParticles != null && !waterParticles.isPlaying)
        {
            waterParticles.Play();
        }

        if (waterClip != null)
        {
            AudioManager.instance.PlayLoopingSFX(waterClip, soundOrigin, 1f);
        }
    }

    void StopWatering()
    {
        isWatering = false;
        if (waterParticles != null && waterParticles.isPlaying)
        {
            waterParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (waterClip != null)
            {
                AudioManager.instance.StopLoopingSFX();
            }
        }
    }

    void WaterPlantsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, wateringRadius, plantLayerMask);

        foreach (Collider col in hits)
        {
            IWaterable plant = col.GetComponent<IWaterable>();  

            if (plant != null && !plant.IsWatered)
            {
                int waterInFrame = Mathf.CeilToInt(waterPerSecond * Time.deltaTime);
                for(int i = 0; i < waterInFrame; i++)
                {
                    plant.ProcessWatering();
                }
            }
        }
    }
    void GameOver()
    {

        playerMover.isOn = false;
        Cursor.lockState = CursorLockMode.None;
        failurePanel.SetActive(true);

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wateringRadius);
    }
}
