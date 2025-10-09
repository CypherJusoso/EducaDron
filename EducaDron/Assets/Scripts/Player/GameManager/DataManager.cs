using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string userId;
    public string username;
    public string email;

    public int currentLvl;
    public int levelPoints;
    public int quizPoints;
    public int TotalPoints => levelPoints + quizPoints;

    public static DataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetPoints()
    {
        levelPoints = 0;
        quizPoints = 0;
    }
}
