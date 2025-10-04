// InformationPresenter.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;

    [Header("Scroll pieces")]
    [SerializeField] private ScrollRect scroll;         // InfoHolder (ScrollRect)
    [SerializeField] private RectTransform viewport;    // Sliding Area (Viewport)
    [SerializeField] private RectTransform contentRoot; // I_ContentArea (Content)

    [Header("Data")]
    [SerializeField] private List<TeoricoSO> teoricos = new();
    [SerializeField] private bool useDataManager = true;
    [SerializeField] private int debugLevel = 1;

    private void Start()
    {
        int level = GetCurrentLevel();
        var data = teoricos.FirstOrDefault(t => t.nivelTeorico == level);
        if (data == null) { Debug.LogWarning($"Sin Teorico para nivel {level}"); return; }

        titleText.text = data.titulo;
        contentText.text = data.contenido;

        // Asegurar referencias por si algo quedó sin asignar en el inspector
        if (scroll)
        {
            if (!scroll.viewport && viewport) scroll.viewport = viewport;
            if (!scroll.content && contentRoot) scroll.content = contentRoot;
        }

        StartCoroutine(ScrollToTopAfterLayout());
    }
    private IEnumerator ScrollToTopAfterLayout()
    {
        // Esperar a que TMP + LayoutGroup calculen tamaños
        yield return new WaitForEndOfFrame();   // <-- antes te puse un ">" de más

        Canvas.ForceUpdateCanvases();

        if (contentText)
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.rectTransform);

        if (contentRoot)
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRoot);

        // Llevar al top (1 = arriba, 0 = abajo)
        if (scroll)
            scroll.verticalNormalizedPosition = 1f;
    }

    private int GetCurrentLevel()
    {
        if (!useDataManager) return debugLevel;
        try
        {
            var t = typeof(DataManager);
            var f = t.GetField("currentLevel");
            if (f != null && f.IsStatic) return (int)f.GetValue(null);

            var pI = t.GetProperty("Instance");
            if (pI != null)
            {
                var inst = pI.GetValue(null);
                var pL = t.GetProperty("CurrentLevel") ?? t.GetProperty("currentLevel");
                if (pL != null) return (int)pL.GetValue(inst);
            }
        }
        catch { }
        return debugLevel;
    }
}
