using Assets.Logic.API;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RegisterApi : MonoBehaviour
{
    string URL = ApiConfig.Build(ApiRoutes.Users.Register);
    
    [SerializeField] GameObject successPanel;
    [SerializeField] TextMeshProUGUI errorText;

    // Spinner a mostrar mientras se hace la request
    [SerializeField] GameObject loadingSpinner;

    public void SendDto(string name, string email, string password, string confirmPassword)
    {
        StartCoroutine(RegisterPost(name, email, password, confirmPassword));
    }

    IEnumerator RegisterPost(string name, string email, string password, string confirmPassword)
    {
        string jsonBody = JsonUtility.ToJson(new RegisterDto(name, email, password, confirmPassword));
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
        
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + req.error);
            
                ErrorResponse errorResponse = null;
                try { errorResponse = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text); } catch { /* ignorar parse error */ }

                if (errorResponse != null && errorResponse.errors != null && errorResponse.errors.Length > 0 && errorText != null)
                {
                    errorText.text = string.Join("\n", errorResponse.errors);
                    errorText.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Respuesta del servidor: " + req.downloadHandler.text);
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
    public class RegisterDto
    {
        public string name;
        public string email;
        public string password;
        public string confirmPassword;

        public RegisterDto(string name, string email, string password, string confirmPassword)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            this.confirmPassword = confirmPassword;
        }
    }

    [System.Serializable]
    public class ErrorResponse
    {
        public string[] errors;
    }
}