using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [SerializeField] PlayerMover3 playerMover;
    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Metodo que desactiva los controles y sonidos del dron cuando se pierde el desafio
    /// </summary>
    public void ActivateGameOver()
    {
        if (playerMover != null)
        {
            playerMover.isOn = false;
            if (playerMover.droneSoundLoop != null)
            {
                playerMover.droneSoundLoop.Stop();
            }
        }
        Debug.Log("Activate Game Over");
    }
}
