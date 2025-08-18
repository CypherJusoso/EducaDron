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

        //Si el input no esta vacío le asigno el string al data manager para que lo pase a la siguiente escena
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
          {
            if(password != confirmPassword)
            {
                Debug.LogWarning("Las contraseñas no coinciden");
            }
            else
            {
                //Llamada POST a la api
                registerApi.SendDto(name, email, password, confirmPassword);
            }
               
          }
          else
          {
              Debug.LogWarning("Debes completar todos los campos.");
          }
      }

    public void GoToLogin()
    {
        //Ir a la siguiente escena
        SceneManager.LoadScene("LoginScene");
    }
}
