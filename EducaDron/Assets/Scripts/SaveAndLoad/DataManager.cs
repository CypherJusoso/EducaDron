using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string username;

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
