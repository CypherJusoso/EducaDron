using UnityEngine;

public class MissionManager : MonoBehaviour
{
    //Se crea una instancia para que se pueda acceder desde otros scripts
    public static MissionManager instance;

    public int totalTargets = 3;
    public int photosTaken = 0;

    [SerializeField] GameObject landingZone;

    private void Awake()
    {
        instance = this;
        landingZone.SetActive(false);
    }

    //Metodo que se ejecuta en photoCapture
    public void OnTargetPhotographed(GameObject target)
    {
        //Buscamos el OnPlantPhoto porque es el que puede decirnos si ya se saco una foto a ese cultivo
        var onPlantPhoto = target.GetComponent<OnPlantPhoto>();
        if (onPlantPhoto != null) 
        {
            bool isPhotographed = onPlantPhoto.OnPhoto();

            if (isPhotographed) 
            {
                photosTaken++;
                Debug.Log("Fotos tomadas: " + photosTaken);
            }
            else
            {
                Debug.Log("Este cultivo ya fue fotografiado!");
            }
        }

        if (photosTaken >= totalTargets)
        {
            Debug.Log("Mision complete");
            landingZone.SetActive(true);
        }
    }
}
