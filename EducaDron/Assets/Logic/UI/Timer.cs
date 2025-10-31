using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float timerDuration = 300.0f;
    [SerializeField] MicroBar timer_MicroBar;
    [SerializeField] Image batteryImage;
    [SerializeField] Sprite[] batterySprites;
    [SerializeField] PlayerMover3 playerMover;
    [SerializeField] GameObject timerLosePanel;

    bool isRunning = false;

    const float MAX_TIME = 300f;
    void Start()
    {
        if (timer_MicroBar != null)
        {
            timer_MicroBar.Initialize(MAX_TIME);
        }
        UpdateBatteryIcon(1f);
    }

    // Update is called once per frame
    void Update()
    {
       
        TimerReduction();
    }
    /// <summary>
    /// Reduce el tiempo por cada frame que pasa y actualiza el icono
    /// de la bateria dependiendo del porcentaje restante
    /// </summary>
    private void TimerReduction()
    {
        if (isRunning)
        {
            timerDuration -= Time.deltaTime;

            if (timerDuration < 0)
            {
                TimerEnd();
            }

            if (timer_MicroBar != null)
            {
                timer_MicroBar.UpdateBar(timerDuration, false, UpdateAnim.Damage);
            }

            float percentage = timerDuration / MAX_TIME;
            UpdateBatteryIcon(percentage);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }
    /// <summary>
    /// Muestra la pantalla de Game Over si se acaba el tiempo
    /// </summary>
    void TimerEnd()
    {
        GameOverManager.instance.ActivateGameOver();
        timerLosePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Time Out");
    }
    /// <summary>
    /// Actualiza el icono de la bateria dependiendo del
    /// tiempo restante
    /// </summary>
    void UpdateBatteryIcon(float percentage)
    {
        if (batterySprites.Length < 4 || batteryImage == null) { return; }
    
        if (percentage > 0.75f)
        {
            batteryImage.sprite = batterySprites[0];
        }
        else if (percentage > 0.5f)
        {
            batteryImage.sprite = batterySprites[1];
        }
        else if (percentage > 0.25f)
        {
            batteryImage.sprite = batterySprites[2];
        }
        else
        {
            batteryImage.sprite = batterySprites[3];
        }
    }
}
