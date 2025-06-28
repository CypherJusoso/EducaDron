using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] PlayerMover3 playerMover;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines;
    [SerializeField] float textSpeed;
    [SerializeField] ThirdPersonLook thirdPersonLook;
    [SerializeField] CinemachineInputAxisController inputProvider;

    int index;


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        textComponent.text = string.Empty;
        playerMover.GetInputActions().Disable();
        thirdPersonLook.controlsEnabled = false;
        inputProvider.enabled = false;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Si el texto termino de escribirse vas a NextLine()
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            //Si no escribo todo el texto de una
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    //Escribe las letras 1 por 1 con el foreach
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        //El if else decide si el dialogo sigue o ya hay que devolver control al jugador
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            playerMover.GetInputActions().Enable();
            thirdPersonLook.controlsEnabled = true;
            inputProvider.enabled = true;
            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}