using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetProgressApi : MonoBehaviour
{
    [SerializeField] Button level1Btn;
    [SerializeField] Button level2Btn;
    [SerializeField] Button level3Btn;

    string userId;
    string URL = "http://localhost:5062/api/progress/?userId=";

    private void Start()
    {
        userId = DataManager.instance.userId;
        StartCoroutine(FetchProgress());
    }

    IEnumerator FetchProgress()
    {
        string fullURL = URL + userId;
        UnityWebRequest req = UnityWebRequest.Get(fullURL);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en el FetchProgress" + req.error);
        }
        else
        {
            Debug.Log("Received JSON: " + req.downloadHandler.text);

            //Respuesta de la api en json
            string json = req.downloadHandler.text;


            //Deserializar 
            ProgressWrapper wrapper = JsonUtility.FromJson<ProgressWrapper>(json);

            LevelProgress[] progress = wrapper.array;

            UpdateButtons(progress);
        }
    }

   

    void UpdateButtons(LevelProgress[] progress)
    {
        foreach (var prog in progress)
        {
            switch (prog.nivel)
            {
                case 1: level1Btn.interactable = !prog.estado.Equals("bloqueado"); break;
                case 2: level2Btn.interactable = !prog.estado.Equals("bloqueado"); break;
                case 3: level3Btn.interactable = !prog.estado.Equals("bloqueado"); break;
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level" + levelNumber);
    }

    [System.Serializable]
    public class LevelProgress
    {
        public int id;
        public string usuarioId;
        public int nivel;
        public string estado;
        public object usuario;
    }

    [System.Serializable]
    public class ProgressWrapper
    {
        public LevelProgress[] array;
    }
}
