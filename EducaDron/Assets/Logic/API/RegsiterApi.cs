using Assets.Logic.API;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterApi : MonoBehaviour
{
    string URL = ApiConfig.Build(ApiRoutes.Users.Register);

    [Header("UI de éxito / carga")]
    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject loadingSpinner;

    [Header("UI de error")]
    // Panel contenedor del error (debe estar inactivo por defecto en la escena)
    [SerializeField] GameObject errorPanel;
    // Texto TMP ubicado dentro del ErrorPanel
    [SerializeField] TextMeshProUGUI errorText;
    // Botón "Entendido" dentro del ErrorPanel
    [SerializeField] Button entendidoBtn;

    void Awake()
    {
        // Estado inicial del panel de error
        if (errorPanel != null) errorPanel.SetActive(false);
        if (errorText != null) errorText.text = string.Empty;

        // Conectar el botón "Entendido" para cerrar el panel
        if (entendidoBtn != null)
        {
            entendidoBtn.onClick.RemoveAllListeners();
            entendidoBtn.onClick.AddListener(HideErrorPanel);
        }
    }

    /// <summary>
    /// Envia los datos y centraliza validaciones locales + manejo de errores de API.
    /// </summary>
    public void SendDto(string name, string email, string password, string confirmPassword)
    {
        // Validación local previa para evitar una request innecesaria
        var validationMessage = GetValidationErrors(name, email, password, confirmPassword);
        if (!string.IsNullOrEmpty(validationMessage))
        {
            ShowErrorPanel(validationMessage);
            return;
        }

        StartCoroutine(RegisterPost(name, email, password, confirmPassword));
    }

    /// <summary>
    /// Valida campos locales. Retorna texto con errores (uno por línea) o vacío si todo es válido.
    /// </summary>
    private string GetValidationErrors(string name, string email, string password, string confirmPassword)
    {
        var sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(name))
            sb.AppendLine("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(email))
            sb.AppendLine("El email es obligatorio.");
        else if (!IsValidEmail(email))
            sb.AppendLine("El email no tiene un formato válido.");

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

    private bool IsValidEmail(string email)
    {
        // Validación simple y robusta para la mayoría de casos.
        const string pattern =
            @"^[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}$";
        return Regex.IsMatch(email ?? string.Empty, pattern);
    }

    /// <summary>
    /// Llama a la API para registrar un nuevo usuario.
    /// </summary>
    IEnumerator RegisterPost(string name, string email, string password, string confirmPassword)
    {
        string jsonBody = JsonUtility.ToJson(new RegisterDto(name, email, password, confirmPassword));
        var req = new UnityWebRequest(URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        req.SetRequestHeader("Content-Type", "application/json");

        // Mostrar spinner y ocultar panel de error previo
        if (loadingSpinner != null) loadingSpinner.SetActive(true);
        HideErrorPanel();

        string pendingErrorMessage = null;

        try
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error de registro: " + req.error);
                pendingErrorMessage = BuildUserFriendlyError(req);
            }
            else
            {
                Debug.Log("Registro OK. Respuesta: " + req.downloadHandler.text);
                HideErrorPanel();
                if (successPanel != null) successPanel.SetActive(true);
            }
        }
        finally
        {
            if (loadingSpinner != null) loadingSpinner.SetActive(false);

            if (!string.IsNullOrEmpty(pendingErrorMessage))
                ShowErrorPanel(pendingErrorMessage);
        }
    }

    private void ShowErrorPanel(string message)
    {
        if (errorText != null) errorText.text = (message ?? string.Empty).Trim();
        if (errorPanel != null) errorPanel.SetActive(true);
    }

    public void HideErrorPanel()
    {
        if (errorPanel != null) errorPanel.SetActive(false);
    }

    private string BuildUserFriendlyError(UnityWebRequest req)
    {
        var raw = req.downloadHandler != null ? req.downloadHandler.text : null;
        ErrorResponse parsed = TryParseErrorResponse(raw);

        // 1) Arreglo de errores del backend
        if (parsed != null && parsed.errors != null && parsed.errors.Length > 0)
            return string.Join("\n", parsed.errors);

        // 2) Mensaje simple o título (problem+json)
        if (parsed != null && !string.IsNullOrEmpty(parsed.message))
            return parsed.message;

        if (parsed != null && !string.IsNullOrEmpty(parsed.title))
            return parsed.title;

        // 3) Cuerpo legible
        if (!string.IsNullOrEmpty(raw))
            return raw;

        // 4) Mensajes por tipo/código de error
        if (req.result == UnityWebRequest.Result.ConnectionError)
            return "No se pudo conectar con el servidor. Verifica tu conexión e inténtalo nuevamente.";

        switch (req.responseCode)
        {
            case 400: return "Datos inválidos. Revisa la información del formulario e inténtalo nuevamente.";
            case 401: return "No autorizado para realizar esta acción.";
            case 403: return "Acceso denegado.";
            case 404: return "Recurso no encontrado.";
            case 409: return "Conflicto al registrar el usuario (posible duplicado).";
            case 422: return "La solicitud contiene datos no procesables. Verifica los campos.";
            case 500: return "Error interno del servidor. Inténtalo más tarde.";
        }

        return $"Ocurrió un problema al procesar tu registro. Código: {req.responseCode}";
    }

    private ErrorResponse TryParseErrorResponse(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try { return JsonUtility.FromJson<ErrorResponse>(json); }
        catch { return null; }
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
        // Soporta tanto problem+json como arrays de errores y mensaje plano:
        // { "title":"...", "status":400, "errors":["..."] }
        // { "message":"...", "errors":["..."] }
        public string title;
        public int status;
        public string message;
        public string[] errors;
    }
}