using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class OnImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image normalImage; 
    [SerializeField] private Image blurImage;   
    [SerializeField] private float transitionDuration = 0.5f;

    [SerializeField] Ease easeType = Ease.InOutSine;

    void Start()
    {
        if (blurImage != null) { blurImage.canvasRenderer.SetAlpha(1f); }
        if(normalImage != null) { normalImage.canvasRenderer.SetAlpha(1f); }

    }

   /// <summary>
   /// Cuando el mouse entra sobre el elemento inicia una
   /// animacion que remueve el desenfoque en la imagen
   /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        blurImage.DOKill();
        blurImage.DOFade(0f, transitionDuration).SetEase(easeType);
    }
    /// <summary>
    /// Cuando el mouse sale del el elemento inicia una
    /// animacion que devuelve el desenfoque en la imagen
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        blurImage.DOKill();
        blurImage.DOFade(1f, transitionDuration).SetEase(easeType);

    }

    void OnDestroy()
    {
        blurImage.DOKill();
    }
}