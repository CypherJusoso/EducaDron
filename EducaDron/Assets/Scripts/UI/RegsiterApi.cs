using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RegisterApi : MonoBehaviour
{
    string URL = "http://localhost:5062/api/users/register";

    [SerializeField] GameObject successPanel;

    public void SendDto(string name, string email, string password, string confirmPassword)
    {
        StartCoroutine(RegisterPost(name, email, password, confirmPassword));
    }

    IEnumerator RegisterPost(string name, string email, string password, string confirmPassword)
    {
        //Crea el objeto RegisterDto y lo pasa a json
        string jsonBody = JsonUtility.ToJson(new RegisterDto(name, email, password, confirmPassword));
        UnityWebRequest req = new UnityWebRequest(URL, "POST");
        
        //Convierte el string json a bytes 
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        //Leer la respuesta del servidor despues de la request
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");   

        yield return req.SendWebRequest();
        
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            //No devuelve nada por algun motivo, no se si es porque cambia de escena pero lo dudo
            Debug.Log("Respuesta del servidor: " + req.downloadHandler.text);
            successPanel.SetActive(true);
        }
    }

    [System.Serializable]
    public class RegisterDto
    {
        public string name;
        public string email;
        public string password;
        public string confirmPassword;

        public RegisterDto(string name, string email, string password, string confirmPassword)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            this.confirmPassword = confirmPassword;
        }
    }
}