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

    /// <summary>
    /// Metodo usado para el proceso de registro enviando los datos ingresados a <see cref="RegisterPost"/> .
    /// </summary>
    /// 
    public void SendDto(string name, string email, string password, string confirmPassword)
    {
        StartCoroutine(RegisterPost(name, email, password, confirmPassword));
    }
    /// <summary>
    /// Metodo que llama a la API con un POST request para registrar un nuevo usuario.
    /// </summary>
    IEnumerator RegisterPost(string name, string email, string password, string confirmPassword)
    {
        //Crea el objeto RegisterDto y lo pasa a json
        string jsonBody = JsonUtility.ToJson(new RegisterDto(name, email, password, confirmPassword));
        UnityWebRequest req = new UnityWebRequest(URL, "POST");
        
        //Convierte el string json a bytes 
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        //Leer la respuesta del servidor despues de la request
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");   

        yield return req.SendWebRequest();
        
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
            
            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);

            if (errorResponse != null && errorResponse.errors != null && errorResponse.errors.Length > 0)
            {
                errorText.text = string.Join("\n", errorResponse.errors);
                errorText.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Respuesta del servidor: " + req.downloadHandler.text);
            successPanel.SetActive(true);
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