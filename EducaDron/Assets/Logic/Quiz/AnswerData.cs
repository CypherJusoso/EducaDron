using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour {

    #region Variables

    [Header("UI Elements")]
    [SerializeField]    TextMeshProUGUI infoTextObject      = null;
    [SerializeField]    Image           toggle              = null;

    [Header("Textures")]
    [SerializeField]    Sprite          uncheckedToggle     = null;
    [SerializeField]    Sprite          checkedToggle       = null;

    [Header("References")]
    [SerializeField]    GameEvents      events              = null;

    private             RectTransform   _rect               = null;
    public              RectTransform   Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return _rect;
        }
    }

    private             int             _answerIndex        = -1;
    public              int             AnswerIndex         { get { return _answerIndex; } }

    private             bool            Checked             = false;

    #endregion

    /// <summary>
    /// Asigna el texto y el indice de la opcion de respuesta
    /// actualizando los datos en pantalla
    /// </summary>
    public void UpdateData (string info, int index)
    {
        infoTextObject.text = info;
        _answerIndex = index;
    }
    /// <summary>
    /// Reinicia la respuesta a su estado por defecto
    /// </summary>
    public void Reset ()
    {
        Checked = false;
        UpdateUI();
    }
    /// <summary>
    /// Cambia el estado actual de la respuesta entre seleccionada
    /// y no seleccionada y notifica al sistema de eventos del
    /// quiz sobre el cambio
    /// </summary>
    public void SwitchState ()
    {
        Checked = !Checked;
        UpdateUI();

        if (events.UpdateQuestionAnswer != null)
        {
            events.UpdateQuestionAnswer(this);
        }
    }
    /// <summary>
    /// Actualiza el sprite del boton segun el estado actual
    /// de la respuesta.
    /// </summary>
    void UpdateUI ()
    {
        if (toggle == null) return;

        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
    }
}