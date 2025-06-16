using UnityEngine;
using AASave;
public class SaveManager : MonoBehaviour
{
    public SaveSystem saveSystem;

    public void SaveGame()
    {
        saveSystem.Save("playerName", DataManager.instance.username);
        saveSystem.Save("levelNumber", DataManager.instance.currentLvl);
    }

    public void LoadGame()
    {
        DataManager.instance.username = saveSystem.Load("playerName", "Unknown").AsString();
        DataManager.instance.currentLvl = saveSystem.Load("levelNumber", 1);
    }
}
