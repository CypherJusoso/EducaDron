using UnityEngine;

public class OnPlantPhoto : MonoBehaviour
{
    [SerializeField] GameObject exclamation;

     public bool isPhotographed = false;

    ///<summary>
    /// Detecta cuando el cultivo es fotografiado y actualiza su estado
    ///</summary>
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
