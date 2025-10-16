using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.UI;

public class Riego : MonoBehaviour
{
    [Header("Tanque de Agua")]
    public const float WATER_CAPACITY = 300f;
    public float aguaActual = 300f;
    public float velocidadRiego = 10f; // Litros por segundo


    [Header("Riego")]
    public ParticleSystem particulasRiego; // Sistema de partículas de agua
    public KeyCode teclaRiego = KeyCode.R;

    [Header("UI")]
    [SerializeField] MicroBar waterMicroBar;
    [SerializeField] GameObject failurePanel;

    private bool estaRegando = false;

    [SerializeField] PlayerMover3 playerMover;


    private void Start()
    {
        if (waterMicroBar != null) waterMicroBar.Initialize(WATER_CAPACITY);
    }

    void Update()
    {
        if (Dialogue.isDialoguePlaying) { return; }
        if (PauseManager.isPaused) { return; }
        // Iniciar o detener riego
        if (Input.GetKey(teclaRiego) && aguaActual > 0)
        {
            IniciarRiego();
        }
        else
        {
            DetenerRiego();
        }

        // Actualizar barra de agua si existe
       if (waterMicroBar != null)
        {
            waterMicroBar.UpdateBar(aguaActual, false, UpdateAnim.Damage);
        }

        // Verificar si se quedó sin agua
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
        }

        aguaActual -= velocidadRiego * Time.deltaTime;
        aguaActual = Mathf.Max(aguaActual, 0f);
    }

    void DetenerRiego()
    {
        if (estaRegando)
        {
            estaRegando = false;
            particulasRiego.Stop();
        }
    }

    void GameOver()
    {

        playerMover.isOn = false;
       Cursor.lockState = CursorLockMode.None;
        failurePanel.SetActive(true);

    }
}
