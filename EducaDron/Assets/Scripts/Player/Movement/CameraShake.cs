using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeAmount = 0.02f;
    [SerializeField] DroneStatusAndCollision droneStatus;
    [SerializeField] float shakeDuration = 0.2f;

    float shakeTimer;

    Vector3 shakeVector;


    private void Update()
    {
        if (droneStatus.isCollided) 
        {
            shakeTimer = shakeDuration;
            droneStatus.isCollided = false;
        }
        if (shakeTimer > 0)
        {
            //Le doy al vector shake un valor random dentro de una esfera * shake amount
            shakeVector = Random.insideUnitSphere * shakeAmount;
            shakeTimer -= Time.deltaTime;
        }
        //La posición actual recibe el vector que sacude la camara
        transform.localPosition = shakeVector;
    }
}
