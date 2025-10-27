using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string firstScene = "MainMenu";
    void Start()
    {
        SceneManager.LoadScene(firstScene);
    }
}
