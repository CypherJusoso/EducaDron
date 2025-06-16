using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
public class PhotoCapture : MonoBehaviour
{
    [SerializeField] Image photoDisplayArea;
    [SerializeField] GameObject photoContainer;
    [SerializeField] GameObject cameraUI;
    [SerializeField] Camera firstPersonCamera;

    [SerializeField] GameObject cameraFlash;
    [SerializeField] float flashTime;

    [SerializeField] Animator fadingAnimation;

    [SerializeField] AudioSource cameraAudio;

    Texture2D screenCapture;

    bool viewingPhoto;
    bool isPhotoMode = false;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraUI.SetActive(false);
    }

    private void Update()
    {
        //Activa y desactiva el modo camara con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isPhotoMode = !isPhotoMode;
            cameraUI.SetActive(isPhotoMode);
        }

        if (!isPhotoMode) { return; }

        //Si apretas click izquierdo tomas la foto o cerras la interfaz
        if (Input.GetMouseButtonDown(0))
        {
            if (!viewingPhoto) 
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                RemovePhoto();
            }
        }

        IEnumerator CapturePhoto()
        {
            cameraUI.SetActive(false);
            viewingPhoto = true;

            //Espera al final del frame para capturar la pantalla
            yield return new WaitForEndOfFrame();

            //La región a leer es el ancho y largo de la pantalla
            Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

            //ReadPixels guarda la captura de pantalla a textura
            screenCapture.ReadPixels(regionToRead, 0, 0, false);
            screenCapture.Apply();

            DetectTargetHit();
            ShowPhoto();
        }
        void ShowPhoto()
        {
            Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
            photoDisplayArea.sprite = photoSprite;

            photoContainer.SetActive(true);
            StartCoroutine(CameraFlashEffect());

            fadingAnimation.Play("PhotoFade");
        }
    }

    private void DetectTargetHit()
    {
        //Lanza una "linea" invisible desde mi camara hasta el centro de la pantalla
        Ray ray = firstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        //Guarda la info de si la linea choca con algo
        RaycastHit hit;

        //Esto checkea donde golpea mi linea en un rango de maxDistance = 100f
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("Target hit! " + hit.collider.name);
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

    //No consigue el efecto deseado todavía, honestamente no hace falta y podemos removerlo.
    IEnumerator CameraFlashEffect()
    {
        cameraAudio.Play();
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }
    void RemovePhoto()
    {
        viewingPhoto = false;
        photoContainer.SetActive(false);
        cameraUI.SetActive(true);
    }
}
//Cosas a tener en cuenta importantes son el feedback visual para el user, de momento solo sabemos que sucede con la foto por la consola, despues deberíamos implementar algo
//Ya sea un sonido de éxito, que te aparezca la misión como completa de una, que la UI de la cámara se vuelva verde al apuntar al objeto correcto, etc.