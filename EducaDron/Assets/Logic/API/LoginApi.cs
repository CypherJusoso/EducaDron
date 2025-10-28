using Assets.Logic.API;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoginApi : MonoBehaviour
{
    string URL = ApiConfig.Build(ApiRoutes.Users.Login);

    [SerializeField] GameObject successPanel;
    [SerializeField] TextMeshProUGUI errorText;

    /// <summary>
    /// Metodo usado para el login del usuario enviando los datos ingresados a <see cref="LoginPost"/>.
    /// </summary>
    public void SendDto(string username, string password)
    {
        StartCoroutine(LoginPost(username, password)); 
    }

    /// <summary>
    /// Solicitud POST a la API para que un usuario inicie sesion.
    /// </summary>
    IEnumerator LoginPost(string username, string password)
    {
        string jsonBody = JsonUtility.ToJson(new LoginDto(username, password));

        UnityWebRequest req = new UnityWebRequest(URL, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        string jsonResponse = req.downloadHandler.text;
        Debug.Log("jsonResponse: " + jsonResponse);

        if (req.result != UnityWebRequest.Result.Success)
        {
            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(jsonResponse);
            if (errorResponse.errors.Length > 0)
            {
                errorText.text = errorResponse.errors[0];
                errorText.gameObject.SetActive(true);
            }

            Debug.LogError("Error: " + req.error);

        }
        else
        {
            jsonResponse = req.downloadHandler.text;
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

    [System.Serializable]
    public class ErrorResponse
    {
        public string title;
        public int status;
        public string[] errors;
    }
}
