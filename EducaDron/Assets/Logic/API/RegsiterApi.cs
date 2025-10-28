using Assets.Logic.API;
using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterApi : MonoBehaviour
{
    string URL = ApiConfig.Build(ApiRoutes.Users.Register);

    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject loadingSpinner;

    [Header("UI de error")]
    [SerializeField] ErrorPanelController errorUi;

    /// <summary>
    /// Metodo usado para el proceso de registro enviando los datos ingresados a <see cref="RegisterPost"/>.
    /// </summary>
    public void SendDto(string name, string email, string password, string confirmPassword)
    {
        var validationMessage = GetValidationErrors(password, confirmPassword);
        if (!string.IsNullOrEmpty(validationMessage))
        {
            errorUi?.Show(validationMessage);
            return;
        }

        StartCoroutine(RegisterPost(name, email, password, confirmPassword));
    }

    /// <summary>
    /// Regresa mensaje de error si la contraseña no cumple con los requisitos. Vacío si es válida.
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
            sb.AppendLine("Las contraseñas no coinciden.");

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Metodo que llama a la API con un POST request para registrar un nuevo usuario.
    /// </summary>
    IEnumerator RegisterPost(string name, string email, string password, string confirmPassword)
    {
        string jsonBody = JsonUtility.ToJson(new RegisterDto(name, email, password, confirmPassword));
        UnityWebRequest req = new UnityWebRequest(URL, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");

        if (loadingSpinner != null) loadingSpinner.SetActive(true);
        errorUi?.Hide();

        string pendingErrorMessage = null;

        try
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + req.error);
                pendingErrorMessage = ApiErrorUtils.BuildUserFriendlyError(req);
            }
            else
            {
                Debug.Log("Respuesta del servidor: " + req.downloadHandler.text);
                if (successPanel != null) successPanel.SetActive(true);
            }
        }
        finally
        {
            if (loadingSpinner != null) loadingSpinner.SetActive(false);
            if (!string.IsNullOrEmpty(pendingErrorMessage))
                errorUi?.Show(pendingErrorMessage);
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
}