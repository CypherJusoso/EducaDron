using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.UI;

public class Riego : MonoBehaviour
{
    [Header("Tanque de Agua")]
    public const float WATER_CAPACITY = 50f;
    public float aguaActual = 50f;
    public float velocidadRiego = 10f; // Litros por segundo

    [Header("Riego")]
    public ParticleSystem particulasRiego; // Sistema de partículas de agua
    public KeyCode teclaRiego = KeyCode.R;

    // NEW: Reference to the trigger object (WaterStreamTrigger)
    [SerializeField] private GameObject waterTriggerObject;

    [Header("UI")]
    [SerializeField] MicroBar waterMicroBar;

    private bool estaRegando = false;

    private void Start()
    {
        if (waterMicroBar != null) waterMicroBar.Initialize(WATER_CAPACITY);

        // Crucial: Ensure the trigger is OFF at the start of the game
        if (waterTriggerObject != null)
        {
            waterTriggerObject.SetActive(false);
        }
    }

    void Update()
    {
        // 1. Activation Check: R key held AND water is available
        if (Input.GetKey(teclaRiego) && aguaActual > 0)
        {
            IniciarRiego();
        }
        // 2. Deactivation Check: R key released OR water ran out
        else
        {
            DetenerRiego();
        }

        // Update the water bar visually
        if (waterMicroBar != null)
        {
            waterMicroBar.UpdateBar(aguaActual, false, UpdateAnim.Damage);
        }

        // Final check for Game Over state
        if (aguaActual <= 0 && estaRegando)
        {
            DetenerRiego();
            GameOver();
        }
    }

    void IniciarRiego()
    {
        if (!estaRegando)
        {
            estaRegando = true;
            particulasRiego.Play();

            // FIX: Activate the trigger object when watering starts
            if (waterTriggerObject != null)
            {
                waterTriggerObject.SetActive(true);
            }
        }

        // Reduce water amount
        aguaActual -= velocidadRiego * Time.deltaTime;
        aguaActual = Mathf.Max(aguaActual, 0f); // Prevents negative water values
    }

    void DetenerRiego()
    {
        if (estaRegando)
        {
            estaRegando = false;
            particulasRiego.Stop();

            // FIX: Deactivate the trigger object when watering stops
            if (waterTriggerObject != null)
            {
                waterTriggerObject.SetActive(false);
            }
        }
    }

    void GameOver()
    {
        Debug.Log("¡Sin agua! Nivel terminado.");
    }
}