using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
   public Vector2 MoveInput {  get; private set; } = Vector2.zero;
   public Vector2 LookInput { get; private set; } = Vector2.zero;

    public float ascendPressed;
    public float descendPessed;
    
    public bool isDroneOn;
    public bool menuOpenInput;
    public bool isFlashlightOn;
   
    InputActions _input;

    /// <summary>
    /// Habilita los inputs del usuario
    /// </summary>
    private void OnEnable()
    {
        _input = new InputActions();
        _input.Player.Enable();

        _input.Player.Move.performed += SetMove;
        _input.Player.Move.canceled += SetMove;

        _input.Player.Look.performed += SetLook;
        _input.Player.Look.canceled += SetLook;

        _input.Player.Ascend.performed += SetAscend;
        _input.Player.Ascend.canceled += SetAscend;

        _input.Player.Descend.performed += SetDescend;
        _input.Player.Descend.canceled += SetDescend;

        _input.Player.ToggleDrone.performed += SetToggleDrone;

        _input.Player.MenuOpen.performed += SetMenuOpen;

        _input.Player.Flashlight.performed += SetPerformed;

    }
    /// <summary>
    /// Deshabilita los inputs del usuario
    /// </summary>
    private void OnDisable()
    {

        _input.Player.Move.performed -= SetMove;
        _input.Player.Move.canceled -= SetMove;

        _input.Player.Look.performed -= SetLook;
        _input.Player.Look.canceled -= SetLook;

        _input.Player.Disable();
    }
    /// <summary>
    ///  Actualiza el vector de movimiento con el input del jugador
    /// </summary>
    void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    /// <summary>
    /// Actualiza el vector de rotacion de camara con el input del jugador
    /// </summary>
    void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }
    /// <summary>
    /// Detecta cuando la tecla para ascender es presionada
    /// </summary>
    void SetAscend(InputAction.CallbackContext ctx)
    {
        ascendPressed = ctx.ReadValue<float>();
    }
    /// <summary>
    /// Detecta cuando la tecla para descender es presionada
    /// </summary>
    void SetDescend(InputAction.CallbackContext ctx) 
    {
        descendPessed = ctx.ReadValue<float>();
    }
    /// <summary>
    /// Detecta cuando la tecla para encender/apagar el dron es presionada
    /// </summary>
    void SetToggleDrone(InputAction.CallbackContext ctx)
    {
        isDroneOn = ctx.performed;
    }
    /// <summary>
    /// Detecta cuando se apreta la tecla para abrir/cerrar el menu de pausa
    /// </summary>
    void SetMenuOpen(InputAction.CallbackContext ctx)
    {
        menuOpenInput = ctx.performed;
    }
    /// <summary>
    /// Detecta cuando se apreta la tecla para encender/apagar la linterna
    /// </summary>
    void SetPerformed(InputAction.CallbackContext ctx)
    {
        isFlashlightOn = ctx.performed;
    }
    /// <summary>
    /// Habilita todos los inputs del usuario
    /// </summary>
    public void EnableInputs()
    {
        _input.Player.Enable();
    }
    /// <summary>
    /// Deshabilita los inputs del usuario
    /// </summary>
    public void DisableInputs()
    {
        _input.Player.Disable();
    }
}
