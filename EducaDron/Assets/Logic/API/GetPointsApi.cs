using Assets.Logic.API;
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

    string URL = ApiConfig.Build(ApiRoutes.Users.Points);

    private void Start()
    {
        userId = DataManager.instance.userId;
        SendGet();

    }
    /// <summary>
    /// Metodo usado para conseguir los puntos de un usuario llamando a <see cref="GetPoints"/>.
    /// </summary>
    public void SendGet()
    {
        StartCoroutine(GetPoints());
    }

    /// <summary>
    /// Metodo que llama a la API con una GET request para conseguir los puntos de un usuario.
    /// </summary>
    IEnumerator GetPoints()
    {
        string fullUrl = URL + userId;
        UnityWebRequest req = UnityWebRequest.Get(fullUrl);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en el Get" + req.error);
        }
        else
        {
            string json = req.downloadHandler.text;
            Debug.Log("Received JSON: " + json);

            UserAllPoints userAllPoints = JsonUtility.FromJson<UserAllPoints>(json);
            UpdateTMP(userAllPoints);
        }
    }

    /// <summary>
    /// Metodo que actualiza el texto mostrando los puntos del jugador en cada nivel.
    /// </summary>
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