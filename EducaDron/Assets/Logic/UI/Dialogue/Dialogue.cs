using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] PlayerMover3 playerMover;
    [SerializeField] ThirdPersonLook thirdPersonLook;
    [SerializeField] CinemachineInputAxisController inputProvider;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] Timer timer;

    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] Transform imagesContainer;
    [SerializeField] Button buttonNext;
    [SerializeField] Button buttonPrevious;

    [SerializeField] TutorialPage[] pages;

    public static bool isDialoguePlaying = false;

    int currentPage = 0;


    void Start()
    {
        isDialoguePlaying=true;
        inputHandler.DisableInputs();
        thirdPersonLook.controlsEnabled = false;
        inputProvider.enabled = false;
        Cursor.lockState = CursorLockMode.None;

        ShowPage(0);

        buttonNext.onClick.AddListener(NextPage);
        buttonPrevious.onClick.AddListener(PrevPage);
    }
    /// <summary>
    /// Muestra una pagina especifica del tutorial, cambiando el texto y las imagenes
    /// segun el indice recibido
    /// </summary>
    void ShowPage(int pageIndex)
    {
        currentPage = pageIndex;
        textComponent.text = pages[pageIndex].text;

        foreach(Transform child in imagesContainer)
        {
            child.gameObject.SetActive(false);
        }

        foreach(var img in pages[pageIndex].images)
        {
            img.SetActive(true);
        }

        buttonPrevious.gameObject.SetActive(pageIndex > 0);
        buttonNext.GetComponentInChildren<TextMeshProUGUI>().text =
            (pageIndex == pages.Length - 1) ? "Comenzar" : "Siguiente";
    }
    /// <summary>
    /// Avanza a la siguiente pagina del tutorial, al llegar
    /// al final reactiva los controles del jugador y comienza el desafio
    /// </summary>
    void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            ShowPage(currentPage + 1);
        }
        else
        {
            isDialoguePlaying = false;
            inputHandler.EnableInputs();
            thirdPersonLook.controlsEnabled = true;
            inputProvider.enabled = true;
            timer.StartTimer();

            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    /// <summary>
    /// Retrocede a la pagina anterior del tutorial si existe
    /// </summary>
    void PrevPage()
    {
        if (currentPage > 0)
        {
            ShowPage(currentPage - 1);
        }
    }
}

[System.Serializable]
public class TutorialPage
{
    [TextArea(2, 4)]
    public string text;
    public GameObject[] images;
}