using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover2 : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform playerCamera;
    [SerializeField] float sensitivity = 0.2f;
    [SerializeField] public float currentSpeed = 0.0f;
    [SerializeField] float maxFlySpeed = 50f;
    [SerializeField] float accelerationRate = 2f;
    [SerializeField] float decelerationRate = 1.0f;
    [SerializeField] Camera firstPersonCamera;
    [SerializeField] CinemachineCamera thirdPersonCamera;

    private DroneController inputActions;

    float xRotation;
    float verticalInput;
    //moveInput pero en 3D
    Vector3 movementInput;
    Vector3 verticalMovementInput;
    Vector3 lastMoveVector;
    Vector2 mouseInput;
    //WASD
    Vector2 moveInput;
    Vector2 lookInput;
    bool ascendPressed;
    bool descendPressed;
    public bool isOn = false; //Activa y desactiva el dron

    private void Awake()
    {
        inputActions = new DroneController();
        /*Cuando un movimiento Move es "performed" paso el contexto que da Unity
         y leo el vector, cuando es cancelado dejo el vector en 0 para detener al jugador*/
        inputActions.Player.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += context => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += context => lookInput = context.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += context => lookInput = Vector2.zero;

        inputActions.Player.Ascend.performed += context => ascendPressed = true;
        inputActions.Player.Ascend.canceled += context => ascendPressed = false;

        inputActions.Player.Descend.performed += context => descendPressed = true;
        inputActions.Player.Descend.canceled += context => descendPressed = false;

        inputActions.Player.ToggleDrone.performed += context => ToggleDrone();
    }

    private void OnEnable() 
    { 
        inputActions.Enable(); 
    }
    private void OnDisable() 
    { 
        inputActions.Disable();
    }

    private void Start()
    {
        firstPersonCamera.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (!isOn) { return; }

        FlyAndInercia();
        MoveCamera();
    }

    private void FlyAndInercia()
    {
        //Valor del teclado
        movementInput = new Vector3(moveInput.x, 0f, moveInput.y);
        
        mouseInput = lookInput;

        //Asignar el valor de y al vector dependiendo de que presione el jugador
        if (ascendPressed)
        { movementInput.y = 1f; }
        else if (descendPressed)
        { movementInput.y = -1f; }
        else
        { movementInput.y = 0f; }

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

    private void FixedUpdate()
    {
        if (!isOn) return;
        MovePlayer();
    }

    void ToggleDrone()
    {
        isOn = !isOn;
        Debug.Log("Drone isOn: " + isOn);

        // Alternar entre cámaras
        firstPersonCamera.enabled = isOn;
        thirdPersonCamera.enabled = !isOn;
    }

    void MovePlayer()
    {
        Vector3 moveVector = transform.TransformDirection(lastMoveVector);

        //Mover el rigidbody en la dirección a la que apunta el vector con tal velocidad
        rb.linearVelocity = moveVector * currentSpeed;
    }

    void MoveCamera()
    {
        xRotation -= mouseInput.y * sensitivity;
        //Limita la rotación con clamp
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, mouseInput.x * sensitivity, 0f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void Save(ref PlayerSaveData data)
    {
        data.position = transform.position;
    }

    public void Load(ref PlayerSaveData data)
    {
        transform.position = data.position;
    }
}


[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 position;
}