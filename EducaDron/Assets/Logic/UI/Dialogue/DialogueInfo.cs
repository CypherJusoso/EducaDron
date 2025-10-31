using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInfo : MonoBehaviour
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

    [SerializeField] GameObject panelTutorial;

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
    /// al final cierra el panel actual y activa el panel del tutorial
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
            gameObject.SetActive(false);
            panelTutorial.SetActive(true);
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
public class InfoPage
{
    [TextArea(2, 4)]
    public string text;
    public GameObject[] images;
}