using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject flashlight;
    [SerializeField] InputHandler _input;
    [SerializeField] PlayerMover3 playerInputs;

    bool flashing = false;

    void Update()
    {
        if (!playerInputs.isOn) { return; }
        if (_input.isFlashlightOn)
        {
            ActivateFlashlight();
            _input.isFlashlightOn = false;
        }
    }

    private void ActivateFlashlight()
    {
        flashing = !flashing;
        flashlight.SetActive(flashing);
    }
}
