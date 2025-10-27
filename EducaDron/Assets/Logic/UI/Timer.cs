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
    [SerializeField] GameSceneManager gameSceneManager;

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

    void TimerEnd()
    {
        playerMover.isOn = false;
        gameSceneManager.GameOver();
        Debug.Log("Time Out");
    }

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
