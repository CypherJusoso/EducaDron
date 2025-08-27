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
   
    InputActions _input;

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

    }

    private void OnDisable()
    {

        _input.Player.Move.performed -= SetMove;
        _input.Player.Move.canceled -= SetMove;

        _input.Player.Look.performed -= SetLook;
        _input.Player.Look.canceled -= SetLook;

        _input.Player.Disable();
    }

    void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }

    void SetAscend(InputAction.CallbackContext ctx)
    {
        ascendPressed = ctx.ReadValue<float>();
    }

    void SetDescend(InputAction.CallbackContext ctx) 
    {
        descendPessed = ctx.ReadValue<float>();
    }

    void SetToggleDrone(InputAction.CallbackContext ctx)
    {
        isDroneOn = ctx.performed;
    }

    void SetMenuOpen(InputAction.CallbackContext ctx)
    {
        menuOpenInput = ctx.performed;
    }

    public void EnableInputs()
    {
        _input.Player.Enable();
    }

    public void DisableInputs()
    {
        _input.Player.Disable();
    }
}
