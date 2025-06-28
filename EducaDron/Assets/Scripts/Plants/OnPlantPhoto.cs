using UnityEngine;

public class OnPlantPhoto : MonoBehaviour
{
    [SerializeField] GameObject exclamation;
    bool isPhotographed = false;

    public bool OnPhoto()
    {
        if (isPhotographed) { return false; }

        isPhotographed = true;

        if (exclamation != null)
        {
            exclamation.SetActive(false);
        }
        return true;
    }
}
