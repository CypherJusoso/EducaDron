using Microlight.MicroBar;
using UnityEngine;

public class Watering : MonoBehaviour
{
    [Header("Tanque de Agua")]
    [SerializeField] float waterCapacity = 1000f;
    public float aguaActual = 1000f;
    public float velocidadRiego = 10f; // Litros por segundo
    private AudioSource currentWaterSound;


    [Header("Riego")]
    [SerializeField] float wateringRadius = 5f;
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

        aguaActual = waterCapacity;

        if (waterMicroBar != null) waterMicroBar.Initialize(waterCapacity);
        
    }
    /// <summary>
    /// Maneja el empezar a regar y detener el riego, 
    /// la reduccion de agua y si se agota el agua
    /// </summary>
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

    /// <summary>
    /// Muestra las particulas de riego y activa el sonido
    /// </summary>
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
    /// <summary>
    /// Cuando se suelta la tecla "R" deja de mostrar las particulas de riego y desactiva el sonido
    /// </summary>
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
    /// <summary>
    /// Detecta a los cultivos dentro del rango y aplica agua gradualmente
    /// a los que no hayan sido regados
    /// </summary>
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

    /// <summary>
    /// Se llama cuando el jugador gasto toda el agua, 
    /// muestra el panel de fallo para volver al menu principal
    /// </summary>
    void GameOver()
    {

        playerMover.isOn = false;
        Cursor.lockState = CursorLockMode.None;
        failurePanel.SetActive(true);

    }
    /// <summary>
    /// Dibuja una esfera en el editor para ver el rango de riego
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wateringRadius);
    }
}
