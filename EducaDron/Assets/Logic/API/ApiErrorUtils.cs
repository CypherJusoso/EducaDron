using UnityEngine.Networking;

public static class ApiErrorUtils
{
    [System.Serializable]
    public class ErrorResponse
    {
        // Soporta problem+json, mensaje plano y arrays de errores.
        public string title;
        public int status;
        public string message;
        public string[] errors;
    }

    public static string BuildUserFriendlyError(UnityWebRequest req)
    {
        var raw = req.downloadHandler != null ? req.downloadHandler.text : null;
        var parsed = TryParse(raw);

        // 1) Preferir lista de errores del backend
        if (parsed != null && parsed.errors != null && parsed.errors.Length > 0)
            return string.Join("\n", parsed.errors);

        // 2) Mensaje simple o título
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
            case 400: return "Datos inválidos o solicitud incorrecta.";
            case 401: return "No autorizado o credenciales incorrectas.";
            case 403: return "Acceso denegado.";
            case 404: return "Recurso no encontrado.";
            case 409: return "Conflicto con los datos enviados.";
            case 422: return "La solicitud contiene datos no procesables.";
            case 429: return "Demasiados intentos. Inténtalo más tarde.";
            case 500: return "Error interno del servidor. Inténtalo más tarde.";
        }

        return $"Ocurrió un problema al procesar la solicitud. Código: {req.responseCode}";
    }

    public static ErrorResponse TryParse(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try { return UnityEngine.JsonUtility.FromJson<ErrorResponse>(json); }
        catch { return null; }
    }
}