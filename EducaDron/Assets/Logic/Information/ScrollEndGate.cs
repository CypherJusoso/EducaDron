using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollEndGate : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private ScrollRect scroll;      // ScrollRect de InfoHolder
    [SerializeField] private GameObject nextButton;  // Next_Button (el GO del botón)
    [SerializeField] private CanvasGroup buttonCg;   // Opcional (para fade y bloqueo)

    [Header("Config")]
    [Tooltip("Se considera 'abajo del todo' cuando verticalNormalizedPosition <= este valor.")]
    [Range(0f, 0.2f)][SerializeField] private float bottomThreshold = 0.02f;

    private bool unlocked;

    private void Awake()
    {
        HideButton();
    }

    private void OnEnable()
    {
        if (scroll) scroll.onValueChanged.AddListener(OnScroll);
        // Si el contenido entra todo en pantalla, el botón se muestra sin obligar a scrollear
        StartCoroutine(LateCheck());
    }

    private void OnDisable()
    {
        if (scroll) scroll.onValueChanged.RemoveListener(OnScroll);
    }

    private IEnumerator LateCheck()
    {
        // Espera a que el layout termine de calcular (TMP/Layouts/ContentSize)
        yield return new WaitForEndOfFrame();

        if (!scroll || !scroll.content || !scroll.viewport) yield break;

        bool needsScroll = scroll.content.rect.height > scroll.viewport.rect.height + 1f;
        if (!needsScroll)
        {
            ShowButton();
            unlocked = true;
        }
    }

    private void OnScroll(Vector2 _)
    {
        if (unlocked || !scroll) return;

        // En ScrollRect: 1 = arriba, 0 = abajo
        if (scroll.verticalNormalizedPosition <= bottomThreshold)
        {
            ShowButton();
            unlocked = true;
        }
    }

    private void ShowButton()
    {
        if (nextButton) nextButton.SetActive(true);
        if (buttonCg)
        {
            buttonCg.alpha = 1f;
            buttonCg.interactable = true;
            buttonCg.blocksRaycasts = true;
        }
    }

    private void HideButton()
    {
        if (nextButton) nextButton.SetActive(false);
        if (buttonCg)
        {
            buttonCg.alpha = 0f;
            buttonCg.interactable = false;
            buttonCg.blocksRaycasts = false;
        }
        unlocked = false;
    }

    // Llamalo si cambiás el texto dinámicamente y querés que vuelva a exigir scroll.
    public void ResetGate()
    {
        HideButton();
        StopAllCoroutines();
        StartCoroutine(LateCheck());
    }
}
