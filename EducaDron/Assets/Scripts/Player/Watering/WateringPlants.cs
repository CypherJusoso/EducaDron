using UnityEngine;
using UnityEngine.UI; // Si usas UI para mostrar el agua restante

public class Riego : MonoBehaviour
{
    [Header("Tanque de Agua")]
    public float capacidadAgua = 100f;
    public float aguaActual = 100f;
    public float velocidadRiego = 10f; // Litros por segundo

    [Header("Riego")]
    public ParticleSystem particulasRiego; // Sistema de partículas de agua
    public KeyCode teclaRiego = KeyCode.R;

    [Header("UI")]
    public Slider barraAgua; // Para mostrar visualmente el agua restante

    private bool estaRegando = false;

    void Update()
    {
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
        if (barraAgua != null)
        {
            barraAgua.value = aguaActual / capacidadAgua;
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
        Debug.Log("¡Sin agua! Nivel terminado.");
        // Aquí puedes poner lógica para terminar el nivel o mostrar UI de Game Over
    }
}
