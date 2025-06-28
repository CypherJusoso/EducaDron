using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonLook : MonoBehaviour
{
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float zoomLerpSpeed = 10f;
    [SerializeField] float minDistance = 3f;
    [SerializeField] float maxDistance = 15f;

    DroneController droneController;

    CinemachineCamera cam;
    CinemachineOrbitalFollow orbital;
    Vector2 scrollDelta;

    float targetZoom;
    float currentZoom;

    public bool controlsEnabled = true;

    void Start()
    {
        droneController = new DroneController();
        droneController.Enable();
        droneController.Camera.Zoom.performed += HandleMouseScroll;
        cam = GetComponent<CinemachineCamera>();
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();

        targetZoom = currentZoom = orbital.Radius;
    }

    private void HandleMouseScroll(InputAction.CallbackContext context)
    {
        scrollDelta = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (!controlsEnabled) { return; }

        if (scrollDelta.y != 0) 
        {
            if (orbital != null) 
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minDistance, maxDistance);
                scrollDelta = Vector2.zero;
            }
        }

        float bumperDelta = droneController.Camera.Zoom.ReadValue<float>();
        if (bumperDelta != 0)
        {
            targetZoom = Mathf.Clamp(orbital.Radius - bumperDelta * zoomSpeed, minDistance, maxDistance);
        }
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        orbital.Radius = currentZoom;
    }
}
