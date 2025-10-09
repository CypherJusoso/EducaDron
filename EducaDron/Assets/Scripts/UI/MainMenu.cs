using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Button SelectButton;

    private void Start()
    {
        SelectButton.interactable = false;

        if (!string.IsNullOrEmpty(DataManager.instance.userId))
        {
            SelectButton.interactable = true;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }

    public void Login()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level3");

    }
    public void Quiz()
    {
        SceneManager.LoadScene("Quiz");

    }
}
