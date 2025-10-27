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

    // Spinner a mostrar mientras se hace la request
    [SerializeField] GameObject loadingSpinner;

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

        // Mostrar spinner y ocultar error previo
        if (loadingSpinner != null) loadingSpinner.SetActive(true);
        if (errorText != null) errorText.gameObject.SetActive(false);

        try
        {
            yield return req.SendWebRequest();

            string jsonResponse = req.downloadHandler.text;
            Debug.Log("jsonResponse: " + jsonResponse);

            if (req.result != UnityWebRequest.Result.Success)
            {
                ErrorResponse errorResponse = null;
                try { errorResponse = JsonUtility.FromJson<ErrorResponse>(jsonResponse); } catch { /* ignorar parse error */ }

                if (errorResponse != null && errorResponse.errors != null && errorResponse.errors.Length > 0 && errorText != null)
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

                if (successPanel != null) successPanel.SetActive(true);
            }
        }
        finally
        {
            // Asegurar ocultar el spinner pase lo que pase
            if (loadingSpinner != null) loadingSpinner.SetActive(false);
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
