using UnityEngine;

public class Rotate : MonoBehaviour
{
    /// <summary>
    /// Rota el simbolo de exclamacion que esta encima de el cultivo objetivo
    /// </summary>
    private void Update()
    {
        
        transform.Rotate(Vector3.up * 45f * Time.deltaTime);
    }
}
