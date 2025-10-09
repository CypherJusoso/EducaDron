using UnityEngine;

public class LevelPointsManager : MonoBehaviour
{

    [SerializeField] DroneStatusAndCollision droneLife;
    [SerializeField] Timer timer;
    [SerializeField] PhotoCapture photoCapture;

    int points = 70;

    private void Awake()
    {
        DataManager.instance.ResetPoints();
    }

    public void CalculatePointsLevel1()
    {
        if(droneLife.droneLife < 60)
        {
            points -= 10;
        }
        
        if(timer.timerDuration < 90)
        {
            points -= 10;
        }

        if(photoCapture.actualPhotos > 5)
        {
            points -= 10;
        }

        DataManager.instance.levelPoints = points;
        Debug.Log("Points: " + points);
    }
}
