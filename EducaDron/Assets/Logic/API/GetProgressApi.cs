using Assets.Logic.API;
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

    [SerializeField] Image level1Image;
    [SerializeField] Image level2Image;
    [SerializeField] Image level3Image;


    [SerializeField] Sprite candadoAbiertoSprite;
    [SerializeField] Sprite candadoCerradoSprite;
    [SerializeField] Sprite nivelCompletoSprite;




    string userId;
    string URL;

    private void Start()
    {
        userId = DataManager.instance.userId;

        URL = ApiConfig.BuildWithQuery(ApiRoutes.Progress.Base, ("userId", userId));

        StartCoroutine(FetchProgress());
    }

    /// <summary>
    /// Solicitud GET a la API para conseguir el progreso de un usuario.
    /// </summary>
    IEnumerator FetchProgress()
    {
        UnityWebRequest req = UnityWebRequest.Get(URL);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en el FetchProgress" + req.error);
        }
        else
        {

            //Respuesta de la api en json
            string json = req.downloadHandler.text;
            Debug.Log("Received JSON: " + json);


            //Deserializar 
            ProgressWrapper wrapper = JsonUtility.FromJson<ProgressWrapper>(json);

            LevelProgress[] progress = wrapper.array;

            UpdateButtons(progress);
        }
    }


    /// <summary>
    /// Actualiza los botones de elegir nivel dependiendo de el progeso del usuario
    /// </summary>
    void UpdateButtons(LevelProgress[] progress)
    {
        foreach (var prog in progress)
        {
            switch (prog.nivel)
            {
                case 1: 
                    level1Btn.interactable = !prog.estado.Equals("bloqueado");
                    if (prog.estado.Equals("desbloqueado"))
                    {
                        level1Image.sprite = candadoAbiertoSprite;
                    }
                    else
                    {
                        level1Image.sprite= nivelCompletoSprite;
                    }
                        break;
                case 2: 
                    level2Btn.interactable = !prog.estado.Equals("bloqueado");
                    if (prog.estado.Equals("desbloqueado"))
                    {
                        level2Image.sprite = candadoAbiertoSprite;
                    }
                    else if (prog.estado.Equals("bloqueado"))
                    {
                        level2Image.sprite = candadoCerradoSprite;
                    }
                    else
                    {
                        level2Image.sprite = nivelCompletoSprite;
                    }
                        break;
                case 3: 
                    level3Btn.interactable = !prog.estado.Equals("bloqueado");
                    if (prog.estado.Equals("desbloqueado"))
                    {
                        level3Image.sprite = candadoAbiertoSprite;
                    }
                    else if (prog.estado.Equals("bloqueado"))
                    {
                        level3Image.sprite = candadoCerradoSprite;
                    }
                    else
                    {
                        level3Image.sprite = nivelCompletoSprite;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Metodo usado para cargar el nivel seleccionado por el usuario.
    /// </summary>
    public void LoadLevel(int levelNumber)
    {
        //DataManager.instance.currentLvl = levelNumber;
        //Debug.LogWarning("Nivel Setteado:" + DataManager.instance.currentLvl);

        SceneManager.LoadScene("Level"+ levelNumber);
        //SceneManager.LoadScene("Level" + levelNumber);
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
