using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string firstScene = "MainMenu";
    /// <summary>
    /// Se usa en la escena Bootstrap para cargar al instante la escena 
    /// MainMenu
    /// </summary>
    void Start()
    {
        SceneManager.LoadScene(firstScene);
    }
}
