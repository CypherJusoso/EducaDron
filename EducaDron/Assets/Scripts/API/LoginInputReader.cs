using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginInputReader : MonoBehaviour
{
    public LoginApi LoginApi;

    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;

    public void ReadInputField()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        ReadStringInput(username, password);
    }

    public void ReadStringInput(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) 
        {
            Debug.LogWarning("Debes completar todos los campos");
        }
        else
        {
            LoginApi.SendDto(username, password);
        }
    }

    public void GoToChooseLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
