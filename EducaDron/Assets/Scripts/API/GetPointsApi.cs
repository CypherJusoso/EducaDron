using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetPointsApi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI level1Text;
    [SerializeField] TextMeshProUGUI level2Text;
    [SerializeField] TextMeshProUGUI level3Text;

    string userId;

    string URLGet = "http://localhost:5062/api/users/points/";
    public void SendGet()
    {
        StartCoroutine(GetPoints());
    }
    private void Start()
    {
        userId = DataManager.instance.userId;
        SendGet();

    }
    IEnumerator GetPoints()
    {
        string fullUrl = URLGet + userId;
        UnityWebRequest req = UnityWebRequest.Get(fullUrl);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en el FetchProgress" + req.error);
        }
        else
        {
            string json = req.downloadHandler.text;
            Debug.Log("Received JSON: " + json);

            UserAllPoints userAllPoints = JsonUtility.FromJson<UserAllPoints>(json);
            UpdateTMP(userAllPoints);
        }
    }

    void UpdateTMP(UserAllPoints userAllPoints)
    {
        foreach (var point in userAllPoints.points)
        {
            switch (point.level)
            {
                case 1:
                    level1Text.text = $"Puntos: {point.points}";
                    break;
                case 2:
                    level2Text.text = $"Puntos: {point.points}";
                    break;
                case 3:
                    level3Text.text = $"Puntos: {point.points}";
                    break;
            }
        }
    }
}

[System.Serializable]
public class LevelPointsData
{
    public int level;
    public int points;
}

[System.Serializable]
public class UserAllPoints
{
    public string userId;
    public LevelPointsData[] points;
    public int total;
}