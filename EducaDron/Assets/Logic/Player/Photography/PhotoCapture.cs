using System.Collections;
using Unity.Cinemachine;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
public class PhotoCapture : MonoBehaviour
{
    [SerializeField] Image photoDisplayArea;
    [SerializeField] Image cameraOverlay;
    [SerializeField] Sprite normalCamOverlay;
    [SerializeField] Sprite greenCamOverlay;
    [SerializeField] GameObject photoContainer;
    [SerializeField] GameObject cameraUI;
    [SerializeField] GameObject UICanvas;
   // [SerializeField] Camera firstPersonCamera;

   // [SerializeField] GameObject cameraFlash;
    [SerializeField] float flashTime;

    [SerializeField] Animator fadingAnimation;

    [SerializeField] AudioClip cameraAudioClip;

    [SerializeField] GameObject failPanel;

    Texture2D screenCapture;

    Camera mainCam;

    [SerializeField] InputHandler inputHandler;

    const int MAX_PHOTOS = 10;

    public int actualPhotos = 0;

    bool viewingPhoto;
    bool isPhotoMode = false;

    private void Start()
    {
     //   firstPersonCamera.enabled = false;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraUI.SetActive(false);
        mainCam = FindFirstObjectByType<CinemachineBrain>().OutputCamera;

    }
    /// <summary>
    /// Maneja el modo camara, la UI y tomar fotos
    /// </summary>
    private void Update()
    {
        if (Dialogue.isDialoguePlaying) { return; }
        if (PauseManager.isPaused) { return; }
        //Activa y desactiva el modo camara con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isPhotoMode = !isPhotoMode;
            cameraUI.SetActive(isPhotoMode);
        }
        if (isPhotoMode)
        {
            UpdateOverlayColor();
        }
        if (!isPhotoMode) { return; }

        //Si apretas click izquierdo tomas la foto o cerras la interfaz
        if (Input.GetMouseButtonDown(0))
        {
            if (!viewingPhoto) 
            {
                actualPhotos++;
                StartCoroutine(CapturePhoto());

                if (actualPhotos == MAX_PHOTOS && MissionManager.instance.photosTaken < MissionManager.instance.totalTargets)
                {
                    inputHandler.DisableInputs();
                    RemovePhoto();
                    failPanel.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;

                }
            }
            else
            {
                RemovePhoto();
            }
        }
    }
    ///<summary>
    /// Crea una imagen de la pantalla y la guarda como textura,
    /// luego verifica si se fotografio a un objetivo
    /// </summary>
    IEnumerator CapturePhoto()
    {
        UICanvas.SetActive(false);
        cameraUI.SetActive(false);
        viewingPhoto = true;

        //Espera al final del frame para capturar la pantalla
        yield return new WaitForEndOfFrame();

        //La región a leer es el ancho y largo de la pantalla
        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        //ReadPixels guarda la captura de pantalla a textura
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        UICanvas.SetActive(true);

        DetectTargetHit();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySoundFXClip(cameraAudioClip, transform, 1f);
        }
        ShowPhoto();
    }
    ///<summary>
    ///Convierte la captura de pantalla en un sprite y lo muestra como fotografia
    /// </summary>
    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoContainer.SetActive(true);
        // StartCoroutine(CameraFlashEffect());

        fadingAnimation.Play("PhotoFade");
    }
    ///<summary>
    ///Al sacar una foto utiliza un Raycast para detectar si se fotografio al objetivo
    /// </summary>
    private void DetectTargetHit()
    {
        //Lanza una "linea" invisible desde mi camara hasta el centro de la pantalla
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        //Guarda la info de si la linea choca con algo
        RaycastHit hit;

        //Esto checkea donde golpea mi linea en un rango de maxDistance = 100f
        if (Physics.Raycast(ray, out hit, 20f))
        {
            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("Target hit! " + hit.collider.name);
                MissionManager.instance.OnTargetPhotographed(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("Target missed!");
            }
        }
        //Si la línea no choca con nada
        else
        {
            Debug.Log("???");
        }
    }
    ///<summary>
    ///Desactiva la interfaz que muestra la foto
    /// </summary>
    void RemovePhoto()
    {
        viewingPhoto = false;
        photoContainer.SetActive(false);
        cameraUI.SetActive(true);
    }
    ///<summary>
    ///Actualiza el overlay del modo camara a verde cuando estas en rango de un cultivo objetivo
    /// </summary>
    void UpdateOverlayColor()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, 20f))
        {
            if (hit.collider.CompareTag("Target"))
            {
                OnPlantPhoto plant = hit.collider.GetComponent<OnPlantPhoto>();
                if (plant != null && !plant.isPhotographed)
                {
                    if (cameraOverlay.sprite != greenCamOverlay)
                    {
                        cameraOverlay.sprite = greenCamOverlay;
                    }
                    return;
                }
            }
        }

        if (cameraOverlay.sprite != normalCamOverlay)
        {
            cameraOverlay.sprite = normalCamOverlay;
        }
    }
}