using Microlight.MicroBar;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] float timerDuration = 180.0f;
    [SerializeField] MicroBar timer_MicroBar;

    [SerializeField] PlayerMover3 playerMover;
    [SerializeField] GameSceneManager gameSceneManager;

    bool isRunning = false;

    const float MAX_TIME = 180f;
    void Start()
    {
        if (timer_MicroBar != null) timer_MicroBar.Initialize(MAX_TIME);
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
}
