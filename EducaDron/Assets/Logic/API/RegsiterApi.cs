using Assets.Logic.API;
using System.Collections;
using System.Text;
using System.Linq;
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

    /// <summary>
    /// Metodo usado para el proceso de registro enviando los datos ingresados a <see cref="RegisterPost"/> .
    /// </summary>
    /// 
    public void SendDto(string name, string email, string password, string confirmPassword)
    {
        // Validación local previa para evitar una request innecesaria
        var validationMessage = GetValidationErrors(password, confirmPassword);
        if (!string.IsNullOrEmpty(validationMessage))
        {
            if (errorText != null)
            {
                errorText.text = validationMessage;
                errorText.gameObject.SetActive(true);
            }
            return;
        }

        StartCoroutine(RegisterPost(name, email, password, confirmPassword));
    }

    /// <summary>
    /// Regresa mensaje de error si la contraseña no cumple con los requisitos.
    /// Vacío si es válida.
    /// </summary>
    private string GetValidationErrors(string password, string confirmPassword)
    {
        var sb = new StringBuilder();

        if (string.IsNullOrEmpty(password))
        {
            sb.AppendLine("La contraseña es obligatoria.");
        }
        else
        {
            if (password.Length < 8) sb.AppendLine("La contraseña debe tener al menos 8 caracteres.");
            if (!password.Any(char.IsLower)) sb.AppendLine("Debe contener al menos una letra minúscula.");
            if (!password.Any(char.IsUpper)) sb.AppendLine("Debe contener al menos una letra mayúscula.");
            if (!password.Any(char.IsDigit)) sb.AppendLine("Debe contener al menos un número.");
            if (!password.Any(ch => !char.IsLetterOrDigit(ch))) sb.AppendLine("Debe contener al menos un carácter especial.");
        }

        if (password != confirmPassword)
        {
            sb.AppendLine("Las contraseñas no coinciden.");
        }

        return sb.ToString().Trim();
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