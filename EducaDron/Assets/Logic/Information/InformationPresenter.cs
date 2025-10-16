// InformationPresenter.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private bool useDataManager;
    [SerializeField] private int debugLevel;

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
        yield return new WaitForEndOfFrame();

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

        if (!useDataManager)
        {
            Debug.LogWarning($"Nivel Seleccionado (debug): {debugLevel}");
            return debugLevel;
        }

        var dm = DataManager.instance; // Puede ser null si aún no inicializado
        int lvl = dm != null ? dm.currentLvl : debugLevel;

        Debug.LogWarning($"Nivel Seleccionado: {lvl}");
        return lvl;
    }

    public void IrAlDesafio()
    {
        int level = GetCurrentLevel();
        SceneManager.LoadScene("Level" + level);
    }
}
