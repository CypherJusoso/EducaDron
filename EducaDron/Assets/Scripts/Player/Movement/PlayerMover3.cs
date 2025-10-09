using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover3 : MonoBehaviour
{
    Rigidbody _rigidBody = null;
    [SerializeField] public float currentSpeed = 0.0f;
    [SerializeField] float maxFlySpeed = 20f;
    [SerializeField] float accelerationRate = 2f;
    [SerializeField] float decelerationRate = 1.0f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] CinemachineCamera firstPersonCamera;
    [SerializeField] CinemachineCamera thirdPersonCamera;
    [SerializeField] ThirdPersonLook thirdPersonScripts;
    [SerializeField] InputHandler _input;
    [SerializeField] Transform hand;

    public bool isOn;
    //moveInput pero en 3D
    Vector3 movementInput;
    Vector3 lastMoveVector;
    InputActions inputActions;

    //WASD
    Vector2 moveInput;
    Vector2 lookInput;
    float ascendPressed;
    float descendPressed;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        firstPersonCamera.enabled = false;
    }

    private void Start()
    {
    }
    public InputActions GetInputActions()
    {
        return inputActions;
    }
    private void Update()
    {
        ascendPressed =_input.ascendPressed;
        descendPressed = _input.descendPessed;
        moveInput = _input.MoveInput;
        lookInput = _input.LookInput;

        if (_input.isDroneOn)
        {
            ToggleDrone();
            _input.isDroneOn = false;
        }
        if (PauseManager.isPaused) return;

        Fly();
        // MoveCamera();

    }
    private void FixedUpdate()
    {
        if (!isOn) return;
        if (PauseManager.isPaused) return;

        UpdateSpeed();

        MovePlayer();

        MovePlayerWithCamera();

    }
    private void Fly()
    {
        //Valor del teclado
        movementInput = new Vector3(moveInput.x, 0f, moveInput.y);
        
        //Asignar el valor de y al vector dependiendo de que presione el jugador
        if (ascendPressed > 0)
        { movementInput.y = 1f; }
        else if (descendPressed > 0)
        { movementInput.y = -1f; }
        else
        { movementInput.y = 0f; }
    }

    void UpdateSpeed()
    {
        if (movementInput.magnitude > 0.1f)
        {
            //Guardo el ultimo movementInput en lastMove
            lastMoveVector = movementInput;
            currentSpeed += accelerationRate * Time.deltaTime;
        }
        else
        {
            currentSpeed -= decelerationRate * Time.deltaTime;
        }
        //La velocidad esta locked a un número entre 0 y maxFlySpeed
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxFlySpeed);
    }
    void MovePlayer()
    {
        Vector3 move = transform.TransformDirection(lastMoveVector);
        _rigidBody.linearVelocity = move * currentSpeed;

    }

    void ToggleDrone()
    {
        isOn = !isOn;
        Debug.Log("Drone isOn: " + isOn);

        // Alternar entre cámaras
        firstPersonCamera.enabled = isOn;
        thirdPersonCamera.enabled = !isOn;

        thirdPersonScripts.controlsEnabled = !isOn;
        if (!isOn) { _rigidBody.useGravity = true; }
        else { _rigidBody.useGravity = false; }
    }
  
    void MovePlayerWithCamera()
    {
        Vector3 camEuler = firstPersonCamera.transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0f, camEuler.y, 0f);

        float flashlightY = Mathf.DeltaAngle(transform.eulerAngles.y, camEuler.y);

        flashlightY = Mathf.Clamp(flashlightY, -60f, 60f);

        hand.localRotation = Quaternion.Euler(camEuler.x, flashlightY, 0);

    }
}