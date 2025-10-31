using Assets.Logic.API;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginApi : MonoBehaviour
{
    string URL = ApiConfig.Build(ApiRoutes.Users.Login);

    [Header("Campos del formulario")]
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;

    [Header("UI de éxito / carga")]
    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject loadingSpinner;

    [Header("UI de error")]
    [SerializeField] ErrorPanelController errorUi;

    // Método que debe llamarse desde el botón "Iniciar sesión" (sin parámetros)
    public void OnLoginClick()
    {
        var username = usernameInput != null ? usernameInput.text : null;
        var password = passwordInput != null ? passwordInput.text : null;

        // Opcional: normalizar espacios
        username = username?.Trim();
        password = password?.Trim();

        SendDto(username, password);
    }

    /// <summary>
    /// Envía los datos y centraliza validaciones locales + manejo de errores de API.
    /// </summary>
    public void SendDto(string username, string password)
    {
        var validationMessage = GetValidationErrors(username, password);
        if (!string.IsNullOrEmpty(validationMessage))
        {
            errorUi?.Show(validationMessage);
            return;
        }

        StartCoroutine(LoginPost(username, password));
    }

    /// <summary>
    /// Valida campos locales. Retorna texto con errores (uno por línea) o vacío si todo es válido.
    /// </summary>
    private string GetValidationErrors(string username, string password)
    {
        var sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(username))
            sb.AppendLine("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(password))
            sb.AppendLine("La contraseña es obligatoria.");

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Solicitud POST a la API para que un usuario inicie sesión.
    /// </summary>
    IEnumerator LoginPost(string username, string password)
    {
        string jsonBody = JsonUtility.ToJson(new LoginDto(username, password));

        var req = new UnityWebRequest(URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        req.SetRequestHeader("Content-Type", "application/json");

        if (loadingSpinner != null) loadingSpinner.SetActive(true);
        errorUi?.Hide();

        string pendingErrorMessage = null;

        try
        {
            yield return req.SendWebRequest();

            string jsonResponse = req.downloadHandler.text;
            Debug.Log("jsonResponse: " + jsonResponse);

            if (req.result != UnityWebRequest.Result.Success)
            {
                pendingErrorMessage = ApiErrorUtils.BuildUserFriendlyError(req);
                Debug.LogError("Error: " + req.error);
            }
            else
            {
                var loginResponse = JsonUtility.FromJson<LoginResponse>(jsonResponse);

                DataManager.instance.userId = loginResponse.userId;
                DataManager.instance.username = username;

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
