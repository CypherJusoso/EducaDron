using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ProgressUpdateApi : MonoBehaviour
{

    string URLPut = "http://localhost:5062/api/progress/";
    public void SendUpdate(string userId, int levelNumber, string newStatus)
    {
        StartCoroutine(UpdateProgress(userId, levelNumber, newStatus));
    }
    IEnumerator UpdateProgress(string userId, int levelNumber, string newStatus)
    {
        string jsonBody = JsonUtility.ToJson(new UpdateProgressDto(userId, levelNumber, newStatus));

        UnityWebRequest req = new UnityWebRequest(URLPut, "PUT");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            string jsonResponse = req.downloadHandler.text;
            Debug.Log("Respuesta del servidor: " + jsonResponse);
        }
    }
}

[System.Serializable]
public class UpdateProgressDto
{
    public string userId;
    public int levelNumber;
    public string newStatus;

    public UpdateProgressDto(string userId, int levelNumber, string newStatus)
    {
        this.userId = userId;
        this.levelNumber = levelNumber;
        this.newStatus = newStatus;
    }
}
