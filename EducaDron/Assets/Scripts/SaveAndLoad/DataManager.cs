using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string userId;
    public string username;
    public string email;

    public int currentLvl;

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
}
