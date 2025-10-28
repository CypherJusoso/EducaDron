using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputReader : MonoBehaviour
{
    public RegisterApi registerApi;

    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField confirmPasswordInput;

    /// <summary>
    /// Recibe los datos ingresados por el usuario y se los manda a RegisterApi,
    /// donde se realizan todas las validaciones y el manejo del ErrorPanel.
    /// </summary>
    public void ReadInputField()
    {
        string name = nameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        ReadStringInput(name, email, password, confirmPassword);
    }

    public void ReadStringInput(string name, string email, string password, string confirmPassword)
    {
        // Centralizamos validaciones y errores en RegisterApi
        registerApi.SendDto(name, email, password, confirmPassword);
    }

    /// <summary>
    /// Dirige al usuario al login
    /// </summary>
    public void GoToLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }

    /// <summary>
    /// Volver al menu principal
    /// </summary>
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
