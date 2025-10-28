using Assets.Logic.API;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SendPointsApi : MonoBehaviour
{

    string URL = ApiConfig.Build(ApiRoutes.Users.UpdatePoints);

    /// <summary>
    /// Solicitud PUT a la API para actualizar los puntos de un usuario 
    /// </summary>
    public IEnumerator UpdatePoints(string userId, int levelNumber, int points)
    {
        string jsonBody = JsonUtility.ToJson(new UpdatePointsDto(userId, points, levelNumber));

        UnityWebRequest req = new UnityWebRequest(URL, "PUT");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + req.error);
        }
        else
        {
            string jsonResponse = req.downloadHandler.text;
            Debug.Log("Respuesta del servidor: " + jsonResponse);
        }
    }
}


[System.Serializable]
public class UpdatePointsDto
{
    public string UserId;

    public int NewPoints;

    public int LevelId;


    public UpdatePointsDto(string userId, int points, int levelNumber)
    {
        this.UserId = userId;

        this.NewPoints = points;

        this.LevelId = levelNumber;

    }
}