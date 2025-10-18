using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginInputReader : MonoBehaviour
{
    public LoginApi LoginApi;

    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TextMeshProUGUI errorText;

    public void ReadInputField()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        ReadStringInput(username, password);
    }

    public void ReadStringInput(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) 
        {
            Debug.Log("Error activado: " + errorText.text);
            errorText.text = "Error: Todos los campos deben estar completos";
            errorText.gameObject.SetActive(true);
            return;
        }
        else
        {
            LoginApi.SendDto(username, password);
        }
    }

    public void GoToChooseLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
