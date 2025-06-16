using UnityEngine;
using UnityEngine.SceneManagement;
using AASave;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public SaveSystem saveSystem;
    public SaveManager saveManager;
    [SerializeField] Button loadButton;

    private void Start()
    {
        if (!saveSystem.DoesDataExists("playerName"))
        {
            loadButton.interactable = false;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Load()
    {
        saveManager.LoadGame();
        SceneManager.LoadScene(DataManager.instance.currentLvl);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }
}
