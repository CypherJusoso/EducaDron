using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class NameApi : MonoBehaviour
{
    private string URL = "http://localhost:5000/api/clients";

    public void SendName(string name)
    {
        StartCoroutine(PostName(name));
    }

    IEnumerator PostName(string name)
    {
        //Crea el objeto namedata y lo pasa a json
        string jsonBody = JsonUtility.ToJson(new NameData(name));
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
        }
    }

    [System.Serializable]
    public class NameData
    {
        public string name;

        public NameData(string name)
        {
            this.name = name;
        }
    }
}