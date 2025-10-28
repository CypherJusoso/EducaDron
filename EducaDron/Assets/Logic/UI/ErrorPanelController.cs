using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ErrorPanelController : MonoBehaviour
{
    [SerializeField] GameObject errorPanel;          // contenedor (activo=false por defecto)
    [SerializeField] TextMeshProUGUI errorText;      // TMP dentro del panel
    [SerializeField] Button entendidoBtn;            // botón dentro del panel

    void Awake()
    {
        // Intento de auto-asignación si el componente está en el propio panel
        if (errorPanel == null) errorPanel = gameObject;
        if (errorText == null) errorText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (entendidoBtn == null) entendidoBtn = GetComponentInChildren<Button>(true);

        if (errorPanel != null) errorPanel.SetActive(false);
        if (errorText != null) errorText.text = string.Empty;

        if (entendidoBtn != null)
        {
            entendidoBtn.onClick.RemoveAllListeners();
            entendidoBtn.onClick.AddListener(Hide);
        }
    }

    public void Show(string message)
    {
        if (errorText != null) errorText.text = (message ?? string.Empty).Trim();
        if (errorPanel != null) errorPanel.SetActive(true);
    }

    public void Hide()
    {
        if (errorPanel != null) errorPanel.SetActive(false);
    }

    public void ShowFromRequest(UnityWebRequest req)
    {
        Show(ApiErrorUtils.BuildUserFriendlyError(req));
    }
}