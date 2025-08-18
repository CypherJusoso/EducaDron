using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LoginApi : MonoBehaviour
{
    string URL = "http://localhost:5062/api/users/login";

    [SerializeField] GameObject successPanel;

    public void SendDto(string username, string password)
    {
        StartCoroutine(LoginPost(username, password)); 
    }

    IEnumerator LoginPost(string username, string password)
    {
        string jsonBody = JsonUtility.ToJson(new LoginDto(username, password));

        UnityWebRequest req = new UnityWebRequest(URL, "POST");

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

            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(jsonResponse);

            DataManager.instance.userId = loginResponse.userId;
            DataManager.instance.username = username;
            successPanel.SetActive(true);
        }
    }

    [System.Serializable]
    public class LoginDto
    {
        public string username;
        public string password;

        public LoginDto(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string userId;
        public string userName;
    }
}
