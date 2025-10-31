using Assets.Logic.API;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogoutApi : MonoBehaviour
{
    
    string URL = ApiConfig.Build(ApiRoutes.Users.Logout);

    [Header("UI de carga / error (opcional)")]
    [SerializeField] GameObject loadingSpinner;
    [SerializeField] ErrorPanelController errorUi;

    /// <summary>
    /// Llama al endpoint de logout. No requiere body.
    /// </summary>
    public IEnumerator Logout()
    {
        var req = new UnityWebRequest(URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(new byte[0]),
            downloadHandler = new DownloadHandlerBuffer()
        };
        req.SetRequestHeader("Content-Type", "application/json");

        if (loadingSpinner != null) loadingSpinner.SetActive(true);
        errorUi?.Hide();

        string pendingErrorMessage = null;

        try
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                pendingErrorMessage = ApiErrorUtils.BuildUserFriendlyError(req);
                Debug.LogError("Logout error: " + req.error);
            }
            else
            {
                Debug.Log("Logout OK: " + req.downloadHandler.text);
            }
        }
        finally
        {
            if (loadingSpinner != null) loadingSpinner.SetActive(false);
            if (!string.IsNullOrEmpty(pendingErrorMessage))
                errorUi?.Show(pendingErrorMessage);
        }
    }
}