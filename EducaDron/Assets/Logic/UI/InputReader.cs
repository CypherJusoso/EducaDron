using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InputReader : MonoBehaviour
{
    public RegisterApi registerApi;

    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField confirmPasswordInput;
    [SerializeField] TextMeshProUGUI errorText;

    /// <summary>
    /// Recibe los datos ingresados por el usuario y 
    /// se los manda a <see cref="ReadStringInput"/>
    /// para su validacion
    /// </summary>
    public void ReadInputField()
    {
        string name = nameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        ReadStringInput(name, email, password, confirmPassword);
    }
    /// <summary>
    /// Verifica que los campos no esten vacios y que las 
    /// contraseñas coincidan, en caso de error muestra un
    /// mensaje en pantalla
    /// </summary>
      public void ReadStringInput(string name, string email, string password, string confirmPassword)
      {


        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            Debug.Log("Error activado: " + errorText.text);
            errorText.text = "Error: Todos los campos deben estar completos";
            errorText.gameObject.SetActive(true);
            return;
        }
        if (password != confirmPassword)
        {
        Debug.LogError("Error activado: " + errorText.text);
        errorText.text = "Error: Las contraseñas no coinciden";
        errorText.gameObject.SetActive(true);
        return;
        }

        errorText.gameObject.SetActive(false);
        registerApi.SendDto(name, email, password, confirmPassword);
    }
    /// <summary>
    /// Dirige al usuario al login
    /// </summary>
public void GoToLogin()
    {
        //Ir a la siguiente escena
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
